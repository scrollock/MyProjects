using ProcessorIndeed.CommonData;
using ProcessorIndeed.Models.Documents;
using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Processing.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessorIndeed.Processing
{
    public sealed class Processor : ProcessorBase, IDisposable
    {
        private object mStartProcessingLocker;
        private object mServiceDequeueLocker;
        private object mAwaitDequeueLocker;
        private bool mIsStarted;

        public Processor() :
            this(ProcessorFactory.GetServiceQueue(),
                      ProcessorFactory.GetAwaitQueue(),
                      ProcessorFactory.GetUnitSupportPool(),
                      ProcessorFactory.GetProcessingTicket(ProcessorFactory.GetServiceQueue(), ProcessorFactory.GetAwaitQueue()))
        {

        }
        public Processor(IProcessor processor) :
            this(processor.ServiceQueue, processor.AwaitQueue,
                      processor.UnitsPool, processor.ProcessingTicket)
        {

        }
        public Processor(IServiceQueue serviceQueue, IAwaitQueue awaitQueue,
                            IUnitSupportPool unitsPool, IProcessingTicket processingTicket)
        {
            ServiceQueue = serviceQueue;
            AwaitQueue = awaitQueue;
            UnitsPool = unitsPool;
            ProcessingTicket = processingTicket;
            mStartProcessingLocker = new object();
            mServiceDequeueLocker = new object();
            mAwaitDequeueLocker = new object();
            Processing = new List<ITicket>();
            History = new List<ITicket>();
            processingTicket.InitHistory(History);
        }
        public override bool IsStarted => mIsStarted;
        
        public override ICollection<IPosition> GetAllPositions()
        {
            return UnitsPool.Pool.ToList();
        }
        public override int GetCountProcessingTickets()
        {
            return ServiceQueue.CountTicket();
        }
        public override int GetComplitedTickets()
        {
            return History.Count;
        }

        public override int GetCountAwaitTickets()
        {
            return AwaitQueue.CountTicket();
        }

        public override void StartProcessing()
        {
            lock (mStartProcessingLocker)
            {
                mIsStarted = true;
            }
            var tokenSource = new CancellationTokenSource();
            try
            {
                
                tokenSource.Token.ThrowIfCancellationRequested();
                var taskProcessingTicket = StartTicketsProcessing(tokenSource, ProcessingTicketStep);
                var taskAwaitTicketForOperator = StartTicketsProcessing(tokenSource, ProcessingAwaitTicketForOperatorStep);
                var taskAwaitTicketForManager = StartTicketsProcessing(tokenSource, ProcessingAwaitTicketForManagerStep);
                var taskAwaitTicketForDirector = StartTicketsProcessing(tokenSource, ProcessingAwaitTicketForDirectorStep);
                try
                {
                    Task.WaitAll(taskProcessingTicket, taskAwaitTicketForOperator, taskAwaitTicketForManager, taskAwaitTicketForDirector);
                }
                catch (AggregateException ae)
                {
                    var resultError = ErrorHelper.ProcessingErrors(ae);
                    if (!string.IsNullOrEmpty(resultError))
                        throw new Exception(resultError);
                }
            }
            catch (Exception ex)
            {
                StopProcessing(ex.Message, tokenSource.Token);
                StartProcessing();
            }
        }

        public override void StopProcessing(string message, CancellationToken ct)
        {
            StopProcessing(message);
            ct.ThrowIfCancellationRequested();
        }
        public override void StopProcessing(string message)
        {
            lock (mStartProcessingLocker)
            {
                mIsStarted = false;
            }
            Console.WriteLine($"Stoped processing {message}");
        }
        /// <summary>
        ///  Получает входящие запросы из входящей очереди
        /// </summary>
        /// <param name="tokenSource"></param>
        /// <param name="ct"></param>
        /// <param name="delay"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private Task StartTicketsProcessing(CancellationTokenSource tokenSource, Action action)
        {
            return Task.Factory.StartNew(() =>
            {
                var ct = tokenSource.Token;
                ct.ThrowIfCancellationRequested();
                System.Diagnostics.Debug.WriteLine($"Start Processing {action.Method.Name.ToString()}");
                do
                {
                    if (ct.IsCancellationRequested)
                    {
                        StopProcessing($"Cancelled {action.Method.Name.ToString()}", ct);
                        ct.ThrowIfCancellationRequested();
                    }
                    try
                    {
                        action();
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"{action.ToString()} error: {ex.Message}");
                    }
                } while (mIsStarted);
                StopProcessing(action.Method.Name.ToString(), ct);
            },
                    tokenSource.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
        }

        /// <summary>
        /// Если есть свободный оператор, то он обрабатывает входящий запрос. Иначе
        /// запрос попадает в очередь
        /// </summary>
        private void ProcessingTicketStep()
        {
            if (ServiceQueue.QueueTickets.Count > 0)
            {
                var ticket = default(Ticket);
                lock (mServiceDequeueLocker)
                {
                    ticket = ServiceQueue.Dequeue();
                }
                if (ticket == null)
                    return;
                if (UnitsPool.GetIdleOperator() is IPosition position)
                {
                    ProcessingTicket.ProcessingAsync(ticket, position, Processing);
                }
                else
                    AwaitQueue.Enqueue(ticket);
            }
        }
        /// <summary>
        /// Как только оператор освободился он получает запрос из очереди (если не
        /// пустая)
        /// </summary>
        private void ProcessingAwaitTicketForOperatorStep()
        {
            if (UnitsPool.GetIdleOperator() is IPosition position && AwaitQueue.QueueTickets.Count > 0)
            {
                var ticket = default(Ticket);
                lock (mAwaitDequeueLocker)
                {
                    ticket = AwaitQueue.Dequeue();
                }
                if (ticket == null)
                    return;
                ProcessingTicket.ProcessingAsync(ticket, position, Processing);
            }
        }
        /// <summary>
        /// Если запрос в очереди больше Tm времени, то может ответить как свободный
        /// оператор так и свободный менеджер
        /// </summary>
        private void ProcessingAwaitTicketForManagerStep()
        {
            if (UnitsPool.GetIdleOperatorOrManager() is IPosition position && AwaitQueue.QueueTickets.Count > 0)
            {
                var ticket = default(Ticket);
                lock (mAwaitDequeueLocker)
                {
                    ticket = AwaitQueue.DequeueForManager();
                }
                if (ticket == null)
                    return;
                ProcessingTicket.ProcessingAsync(ticket, position, Processing);
            }
        }
        /// <summary>
        /// Если запрос в очереди больше Td, то запрос поступает и к директору
        /// </summary>
        private void ProcessingAwaitTicketForDirectorStep()
        {
            if (UnitsPool.GetDirector() is IPosition position && AwaitQueue.QueueTickets.Count > 0)
            {
                var ticket = default(Ticket);
                lock (mAwaitDequeueLocker)
                {
                    ticket = AwaitQueue.DequeueForDirector();
                }
                if (ticket == null)
                {
                    return;
                }
                ProcessingTicket.ProcessingAsync(ticket, position, Processing);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ServiceQueue.QueueTickets.Clear();
                    AwaitQueue.QueueTickets.Clear();
                    Processing.Clear();
                    History.Clear();
                }
                disposedValue = true;
            }
        }
       
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
using Newtonsoft.Json;
using ProcessorIndeed.Models.Documents;
using ProcessorIndeed.Models.Emploees;
using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Processing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessorIndeed.Processing
{
    public class ProcessingTicket : IProcessingTicket
    {
        public int RundomPeriodSecondes { get; set; }
        public int RundomPeriodMilliSecondes => RundomPeriodSecondes * 1000;
        public IServiceQueue InputQueue { get; }
        public IAwaitQueue AwaitQueue { get; }
        public ICollection<ITicket> History { get; set; }
        public ProcessingTicket(IServiceQueue inputQueue, IAwaitQueue awaitQueue)
        {
            InputQueue = inputQueue;
            AwaitQueue = awaitQueue;
        }
        public void InitHistory(ICollection<ITicket> history)
        {
            History = history;
        }

        public void AddNewTicket(string content)
        {
            try
            {
                var ticket = JsonConvert.DeserializeObject<Ticket>(content);
                lock(ticket.Lock)
                {
                    ticket.id = Guid.NewGuid();
                    ticket.StartProcessing = DateTime.Now;
                }
                InputQueue.Enqueue(ticket);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }

        public void CancellProcessingTicket(ITicket ticket)
        {
            if (ticket == null)
                return;
            ticket.IsCanceled = true;
            ticket.IsCompleted = true;
            ticket.EndProcessing = DateTime.Now;
        }

        public void CancellProcessingTicket(string ids)
        {
            try
            {
                var id = JsonConvert.DeserializeObject<Guid>(ids);
                var ticket = default(Ticket);
                if (id != default(Guid))
                {
                    ticket = InputQueue.QueueTickets.FirstOrDefault(x => x.id == id);
                    if(ticket == null)
                        ticket = AwaitQueue.QueueTickets.FirstOrDefault(x => x.id == id);
                }
                if(ticket != null)
                    CancellProcessingTicket(ticket);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public string GetProcessedTicket(string ids)
        {
            var result = string.Empty;
            try
            {
                var id = JsonConvert.DeserializeObject<Guid>(ids);
                var ticket = default(Ticket);
                if (id != default(Guid))
                {
                    ticket = InputQueue.QueueTickets.FirstOrDefault(x => x.id == id);
                    if (ticket == null)
                        ticket = AwaitQueue.QueueTickets.FirstOrDefault(x => x.id == id);
                }
                
                if (ticket != null)
                    result = JsonConvert.SerializeObject(ticket);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return result;
        }
        public bool IsTicketCompleted(ITicket ticket)
        {
            return ticket?.IsCompleted ?? false;
        }
        public string GetAllProcessingTickets()
        {
            var allTickets = InputQueue.QueueTickets.Select(x => x)
                .Union(AwaitQueue.QueueTickets.Select(x => x)).ToList();
            return JsonConvert.SerializeObject(allTickets);
        }

        public string GetAllHistoryTickets()
        {
            var allTickets = History.Select(x => x).ToList();
            return JsonConvert.SerializeObject(allTickets);
        }
        public void Processing(ITicket ticket, IPosition position, ICollection<ITicket> process)
        {
            if (ticket == null)
                return;
            PrevSetProccessing(ticket, position, process);
            PostSetProccessing(ticket, position, process);
        }
        
        public void ProcessingAsync(ITicket ticket, IPosition position, ICollection<ITicket> process)
        {
            if (ticket == null)
                return;
            Console.WriteLine($"Start processing Ticket {ticket.id} in {DateTime.Now}");
            Task.Factory.StartNew(() =>
            {
                PrevSetProccessing(ticket, position, process);
                Thread.CurrentThread.Join(RundomPeriodMilliSecondes);
                PostSetProccessing(ticket, position, process);
            });
        }
        private void PostSetProccessing(ITicket ticket, IPosition position, ICollection<ITicket> process)
        {
            lock (ticket.Lock)
            {
                ticket.EndProcessing = DateTime.Now;
                ticket.IsCompleted = true;
                ticket.Period = ticket.EndProcessing - ticket.StartProcessing;
            }
            lock (position.Lock)
            {
                position.IsWorkBusy = false;
                position.StartIdle = DateTime.Now;
            }
            process.Remove(ticket);
            History.Add(ticket);
            Console.WriteLine($"Ticket {ticket.id} was processed in {ticket.EndProcessing}");
        }
        private void PrevSetProccessing(ITicket ticket, IPosition position, ICollection<ITicket> process)
        {
            lock (position.Lock)
            {
                position.IsWorkBusy = true;
                position.StartIdle = null;
            }
            lock (ticket.Lock)
            {
                ticket.OwnerPosition = position as Position;
                ticket.CurrentLewelOwner = position.Level;
                ticket.IsCanceled = false;
                ticket.IsCompleted = false;
                ticket.Period = default(TimeSpan);
                ticket.StartProcessing = DateTime.Now;
            }
            process.Add(ticket);
        }

        
    }
}

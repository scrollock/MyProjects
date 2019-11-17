using ProcessorIndeed.CommonData;
using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Processing.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;

namespace ProcessorIndeed.Processing
{
    public sealed class PipeServer : IHostServer, IDisposable
    {
        static IProcessor _processor;
        private bool _disposed = false;
        ServiceHost _serviceHost;
        IStartContent _startContent;
        public bool IsActive {get; private set;}
        public void InitHost()
        {
            _serviceHost = PipeFactory.GetHost(_startContent.PipeAddress.FullAddress);
            var serviceMetadataBehavior = _serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (serviceMetadataBehavior == null)
            {
                serviceMetadataBehavior = new ServiceMetadataBehavior();
                _serviceHost.Description.Behaviors.Add(serviceMetadataBehavior);
            }
            _serviceHost.AddServiceEndpoint(typeof(IMetadataExchange),
                MetadataExchangeBindings.CreateMexNamedPipeBinding(), "net.pipe://localhost/service/mex");
        }
        public void InitProcessor(IStartContent startContent)
        {
            _startContent = startContent;
            _processor = ProcessorFactory.GetProcessor(_startContent);
            
        }
        public void StartProcessing()
        {
            Console.WriteLine("StartProcessing tickets");
            if(_processor != null)
                _processor.StartProcessing();
        }

        public void StopProcessing(string message)
        {
            if(_processor != null)
                _processor.StopProcessing(message);
            Console.WriteLine("StopProcessing tickets");
        }
        public bool OpenHost()
        {
            try
            {
                _serviceHost.Open();
                Console.WriteLine("Pipe server listenen" + _serviceHost.State);
                return IsActive = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsActive = false;
                if (_serviceHost != null)
                {
                    if(_processor.IsStarted)
                    {
                        _processor.StopProcessing(ex.Message);
                        Console.WriteLine("StopProcessing tickets");
                    }
                        
                    _serviceHost.Abort();
                }
                _serviceHost = null;
                throw;
            }
        }
        public void CloseHost()
        {
            Console.WriteLine("CloseHost()");
            IsActive = false;
            if (_serviceHost != null)
            {
                if (_processor?.IsStarted ?? false)
                    _processor.StopProcessing("Stop Processing");
                _serviceHost.Close();
                Thread.CurrentThread.Join(3000);
                if (_serviceHost.State != CommunicationState.Closed)
                {
                    _serviceHost.Abort();
                }
            }
        }

        public IAsyncResult BeginPipeCommand(string command, string content, AsyncCallback callback, object param)
        {
            var result = LiteralStrings.OK;
            try
            {
                switch (command)
                {
                    case nameof(ProcessorCommands.AddNewTicket):
                        _processor.ProcessingTicket.AddNewTicket(content);
                        break;
                    case nameof(ProcessorCommands.CanceledTicket):
                        _processor.ProcessingTicket.CancellProcessingTicket(content);
                        break;
                    case nameof(ProcessorCommands.GetProcessedTicket):
                        result = _processor.ProcessingTicket.GetProcessedTicket(content);
                        break;
                    case nameof(ProcessorCommands.GetAllProcessingTickets):
                        result = _processor.ProcessingTicket.GetAllProcessingTickets();
                        break;
                    case nameof(ProcessorCommands.GetAllHistoryTickets):
                        result = _processor.ProcessingTicket.GetAllHistoryTickets();
                        break;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return GetAsyncResultObject(result);
        }

        public string EndPipeCommand(IAsyncResult asyncResult)
        {
            var callback = (IPipeServer)asyncResult.AsyncState;
            if (callback == null)
                return string.Empty;
            return callback.EndSampleCommand(asyncResult);
        }

        public string PipeCommandSync(string command, string content)
        {
            var result = LiteralStrings.OK;
            try
            {
                switch (command)
                {
                    case nameof(ProcessorCommands.AddNewTicket):
                        _processor.ProcessingTicket.AddNewTicket(content);
                        break;
                    case nameof(ProcessorCommands.CanceledTicket):
                        _processor.ProcessingTicket.CancellProcessingTicket(content);
                        break;
                    case nameof(ProcessorCommands.GetProcessedTicket):
                        result = _processor.ProcessingTicket.GetProcessedTicket(content);
                        break;
                    case nameof(ProcessorCommands.GetAllProcessingTickets):
                        result = _processor.ProcessingTicket.GetAllProcessingTickets();
                        break;
                    case nameof(ProcessorCommands.GetAllHistoryTickets):
                        result = _processor.ProcessingTicket.GetAllHistoryTickets();
                        break;
                    default:
                        return result;
                }
            }
            catch(Exception ex)
            {
                result = ex.Message;
                Console.WriteLine("Error:" + ex.Message);
                return ex.Message;
            }
            return result;
        }

        public string SampleCommand(string command)
        {
            return PipeCommandSync(command, "");
        }

        public IAsyncResult BeginSampleCommand(string command, AsyncCallback callback, object state)
        {
            return BeginPipeCommand(command, "", callback, state);
        }

        public string EndSampleCommand(IAsyncResult result)
        {
            return EndPipeCommand(result);
        }
        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool Disposing)
        {
            if (!_disposed)
            {
                if (Disposing)
                {
                    if (_serviceHost != null)
                    {
                        CloseHost();
                        _serviceHost = null;
                    }
                }
                _disposed = true;
            }
        }

        ~PipeServer()
        {
            Dispose(false);
        }
        #endregion Dispose
        private CompletedAsyncResult<T> GetAsyncResultObject<T>(T data)
        {
            return new CompletedAsyncResult<T>(data);
        }
        private CompletedAsyncResult<ICollection<T>> GetAsyncResultObject<T>(ICollection<T> data) where T: class
        {
            return new CompletedAsyncResult<ICollection<T>>(data);
        }
    }
}

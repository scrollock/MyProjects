using ProcessorIndeed.Models.Documents;
using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Processing;
using ProcessorIndeed.Processing.Interfaces;
using System;
using System.ServiceModel;

namespace SupportIndeed.JetPipe
{
    [ServiceBehavior(
          ConcurrencyMode = ConcurrencyMode.Single,
          InstanceContextMode = InstanceContextMode.PerSession,
          //ReleaseServiceInstanceOnTransactionComplete = true,
          IncludeExceptionDetailInFaults = true
        )]
    public class PipeClient : IPipeServer, IDisposable
    {
        private static IStartContent _startContent;
        private IPipeServer _proxy = null;
        //private IJetCommand _jet = null;
        private bool _disposed = false;
        public PipeClient(IStartContent startContent)
        {
            _startContent = startContent;
            if (_startContent == null)
                return;
            InitProxy();
        }

        public void InitProxy()
        {
             _proxy = PipeFactory.GetPipeClient(new Uri(_startContent.PipeAddress.FullAddress));
        }

        public IJetCommand JetCommander => PipeFactory.GetJetCommand(_proxy);
        #region IPipeServer
        public bool IsActive => _proxy.IsActive;
        public string PipeCommandSync(string command, string content)
        {
            if (_proxy == null)
                return string.Empty;
            return _proxy.PipeCommandSync(command, content);
        }
        public IAsyncResult BeginPipeCommand(string command, string content, AsyncCallback callback, object state)
        {
            var result = default(IAsyncResult);
            try
            {
                result = _proxy.BeginPipeCommand(command, content, callback, state);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return result;
        }
        public string EndPipeCommand(IAsyncResult result)
        {
            if (_proxy == null)
                return string.Empty;
            return _proxy.EndPipeCommand(result);
        }
        public string SampleCommand(string command)
        {
            if (_proxy == null)
                return string.Empty;
            return _proxy.SampleCommand(command);
        }
        public IAsyncResult BeginSampleCommand(string command, AsyncCallback callback, object state)
        {
            if (_proxy == null)
                return null;
            return _proxy.BeginSampleCommand(command, callback, state);
        }
        public string EndSampleCommand(IAsyncResult result)
        {
            if (_proxy == null)
                return string.Empty;
            return _proxy.EndSampleCommand(result);
        }
        #endregion IPipeServer
        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _proxy = null;
                }
                // TODO: free unmanaged resources
                _disposed = true;
            }
        }
         ~PipeClient()
         {
            Dispose(false);
         }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
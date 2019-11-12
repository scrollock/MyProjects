using System;
using System.ServiceModel;

namespace ProcessorIndeed.Processing.Interfaces
{
    [ServiceContract]
    [ServiceKnownType(typeof(string))]
    public interface IPipeServer : IServer
    {
        [OperationContract]
        string PipeCommandSync(string command, string content);
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginPipeCommand(string command,string content, AsyncCallback callback, object state);
        string EndPipeCommand(IAsyncResult result);
    }
}

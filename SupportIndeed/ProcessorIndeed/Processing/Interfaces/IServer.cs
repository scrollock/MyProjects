using System;
using System.ServiceModel;

namespace ProcessorIndeed.Processing.Interfaces
{
    [ServiceContract]
    [ServiceKnownType(typeof(string))]
    public interface IServer
    {
        [OperationContract]
        string SampleCommand(string command);
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginSampleCommand(string commandType, AsyncCallback callback, object state);
        string EndSampleCommand(IAsyncResult result);
        bool IsActive { get; }
    }
}

using System;
using System.Threading;

namespace ProcessorIndeed.CommonData
{
    public class CompletedAsyncResult<T> : IAsyncResult
    {
        T data;

        public CompletedAsyncResult(T data)
        { this.data = data; }

        public T Data
        { get { return data; } }

        #region IAsyncResult Members
        public object AsyncState
        { get { return data; } }

        public WaitHandle AsyncWaitHandle
        { get { throw new Exception("Method or operation hasn't release."); } }

        public bool CompletedSynchronously
        { get { return true; } }

        public bool IsCompleted
        { get { return true; } }
        #endregion

    }
}

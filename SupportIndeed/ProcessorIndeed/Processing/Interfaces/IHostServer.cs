using ProcessorIndeed.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorIndeed.Processing.Interfaces
{
    public interface IHostServer : IPipeServer
    {
        void InitHost();
        void InitProcessor(IStartContent startContent);
        void StartProcessing();
        void StopProcessing(string message);
        bool OpenHost();
        void CloseHost();
    }
}

using ProcessorIndeed.Models.Interfaces;
using System.Collections.Generic;

namespace ProcessorIndeed.Processing.Interfaces
{
    public interface IProcessor
    {
        void StartProcessing();
        void StopProcessing(string message);
        IUnitSupportPool UnitsPool { get; }
        ICollection<IPosition> GetAllPositions();
        ICollection<ITicket> History { get; }
        ICollection<ITicket> Processing { get; }
        IServiceQueue ServiceQueue { get; }
        IAwaitQueue AwaitQueue { get; }
        IProcessingTicket ProcessingTicket { get;}
        int GetCountProcessingTickets();
        int GetCountAwaitTickets();
        int GetComplitedTickets();
        void SetParameters(IStartContent startContent);
        bool IsStarted { get; }
    }
}

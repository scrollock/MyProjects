using System.Collections.Generic;
using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Processing.Interfaces;

namespace ProcessorIndeed.Processing
{
    public abstract class ProcessorBase : IProcessor
    {
        public virtual bool IsStarted { get; }
        public virtual IUnitSupportPool UnitsPool { get; set; }
        public virtual IServiceQueue ServiceQueue { get; set; }
        public virtual IAwaitQueue AwaitQueue { get; set; }
        public virtual IProcessingTicket ProcessingTicket { get; set; }
        public virtual ICollection<ITicket> Processing { get; protected set; }
        public virtual ICollection<ITicket> History { get; protected set; }
        public abstract ICollection<IPosition> GetAllPositions();

        public abstract int GetComplitedTickets();
        public abstract int GetCountAwaitTickets();
        public abstract int GetCountProcessingTickets();

        public abstract void SetParameters(IStartContent startContent);
        public abstract void StartProcessing();
        public abstract void StopProcessing(string message);
    }
}
using ProcessorIndeed.Models.Documents;
using System;

namespace ProcessorIndeed.Processing.Interfaces
{
    public interface IAwaitQueue : IQueue
    {
        Ticket DequeueForManager();
        Ticket DequeueForDirector();
        int Tm { get; set; }
        int Td { get; set; }
    }
}

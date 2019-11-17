using ProcessorIndeed.Models.Documents;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ProcessorIndeed.Processing.Interfaces
{
    public interface IQueue
    {
        ConcurrentQueue<Ticket> QueueTickets { get; }
        Ticket Dequeue();
        void Enqueue(Ticket ticket);
        int CountTicket();
        ICollection<Ticket> GetAllQuene();
    }
}

using ProcessorIndeed.Models.Documents;
using ProcessorIndeed.Processing.Interfaces;
using System.Collections.Generic;

namespace ProcessorIndeed.Processing
{
    public class ServiceQueue : QueueBase, IServiceQueue
    {
        public ServiceQueue()
        {
            QueueTickets = new Queue<Ticket>();
        }
    }
}

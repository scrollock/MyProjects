using ProcessorIndeed.Models.Documents;
using ProcessorIndeed.Processing.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ProcessorIndeed.Processing
{
    public abstract class QueueBase : IQueue
    {

        public ConcurrentQueue<Ticket> QueueTickets { get; protected set; }
        public int CountTicket()
        {
            return QueueTickets?.Count ?? 0;
        }

        public Ticket Dequeue()
        {
            if (QueueTickets == null)
                return null;
            var result = default(Ticket);
            do
            {
                QueueTickets.TryDequeue(out result);
            } while (result.OwnerPosition != null && !result.IsCanceled &&
                     result.CurrentLewelOwner != Models.SupportDivision.LevelPositionEnum.None);
            return result;
        }

        public void Enqueue(Ticket ticket)
        {
            if (QueueTickets == null)
                return;
            QueueTickets.Enqueue(ticket);
        }

        public ICollection<Ticket> GetAllQuene()
        {
            if (QueueTickets == null)
                return new List<Ticket>();
            return QueueTickets.Select(x => x).ToList();
        }
    }
}

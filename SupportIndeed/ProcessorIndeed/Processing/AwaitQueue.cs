using ProcessorIndeed.Models.Documents;
using ProcessorIndeed.Models.SupportDivision;
using ProcessorIndeed.Processing.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace ProcessorIndeed.Processing
{
    public class AwaitQueue : QueueBase, IAwaitQueue
    {
        public int Tm { get; set; }
        public int Td { get; set; }

       
        public AwaitQueue()
        {
            QueueTickets = new ConcurrentQueue<Ticket>();
        }
        public Ticket DequeueForDirector()
        {
            var dateTime = DateTime.Now;
            return QueueTickets.OrderBy(x=>x.StartProcessing)
                .FirstOrDefault(x=> !x.IsCanceled && x.CurrentLewelOwner == LevelPositionEnum.None && (dateTime - x.StartProcessing).TotalMinutes >= Td);
        }

        public Ticket DequeueForManager()
        {
            var dateTime = DateTime.Now;
            return QueueTickets.OrderBy(x => x.StartProcessing)
                .FirstOrDefault(x => !x.IsCanceled && x.CurrentLewelOwner == LevelPositionEnum.None && (dateTime - x.StartProcessing).TotalMinutes >= Tm);
        }
    }
}

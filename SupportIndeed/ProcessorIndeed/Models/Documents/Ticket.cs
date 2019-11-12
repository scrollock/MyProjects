using ProcessorIndeed.Models.Emploees;
using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Models.SupportDivision;
using System;

namespace ProcessorIndeed.Models.Documents
{
    public class Ticket : ObjectBase<ITicket>, ITicket
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public Position OwnerPosition { get; set; }
        public LevelPositionEnum CurrentLewelOwner { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime StartProcessing { get; set; }
        public DateTime EndProcessing { get; set; }
        public TimeSpan Period { get; set; }
        public object Lock => new object();
    }
}
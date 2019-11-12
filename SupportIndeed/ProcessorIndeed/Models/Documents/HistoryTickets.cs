using ProcessorIndeed.Models.Interfaces;
using System;

namespace ProcessorIndeed.Models.Documents
{
    public class HistoryItem : ObjectBase<IHistoryItem>
    {
        public Ticket TicketItem { get; set; }
        public DateTime EndProcessing { get; set; }
    }
}
using ProcessorIndeed.Models.Documents;
using System;

namespace ProcessorIndeed.Models.Interfaces
{
    public interface IHistoryItem : IObjectBase
    {
        Ticket TicketItem { get; set; }
        DateTime EndProcessing { get; set; }
    }
}

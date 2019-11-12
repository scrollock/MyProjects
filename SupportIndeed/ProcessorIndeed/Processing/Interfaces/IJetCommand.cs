using ProcessorIndeed.Models.Documents;
using System;

namespace ProcessorIndeed.Processing.Interfaces
{
    public interface IJetCommand
    {
        Ticket AddNewTicket(string ticket);
        Guid CanceledTicket(Guid id);
        Ticket GetTicket(Guid id);
        string GetAllProcessingTickets();
        string GetAllHistoryTickets();
    }
}

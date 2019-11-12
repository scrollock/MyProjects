using ProcessorIndeed.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ProcessorIndeed.Processing.Interfaces
{
    public interface IProcessingTicket
    {
        int RundomPeriodSecondes { get; set; }
        void Processing(ITicket ticket, IPosition position, ICollection<ITicket> process);
        void ProcessingAsync(ITicket ticket, IPosition position, ICollection<ITicket> process);
        void CancellProcessingTicket(ITicket ticket);
        bool IsTicketCompleted(ITicket ticket);
        void AddNewTicket(string content);
        void CancellProcessingTicket(string id);
        string GetProcessedTicket(string id);
        string GetAllProcessingTickets();
        string GetAllHistoryTickets();
        void InitHistory(ICollection<ITicket> history);
    }
}

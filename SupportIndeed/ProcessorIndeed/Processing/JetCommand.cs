using Newtonsoft.Json;
using ProcessorIndeed.Models.Documents;
using ProcessorIndeed.Processing.Interfaces;
using System;
using System.ServiceModel;

namespace ProcessorIndeed.Processing
{
    public class JetCommand : IJetCommand
    {
        private IPipeServer mServer;
        public JetCommand(IPipeServer server)
        {
            mServer = server;
        }

        public Ticket AddNewTicket(string ticket)
        {
            var ticketResult = JsonConvert.DeserializeObject<Ticket>(ticket);
            System.Diagnostics.Debug.WriteLine("Send AddNewTicket to server");
            mServer.BeginPipeCommand(nameof(ProcessorCommands.AddNewTicket), ticket, TicketCallback, null);
            System.Diagnostics.Debug.WriteLine("started BeginPipeCommand");
            return ticketResult;
        }
        public Ticket GetTicket(Guid id)
        {
            var jsonString = JsonConvert.SerializeObject(id);
            var result = mServer.BeginPipeCommand(nameof(ProcessorCommands.GetProcessedTicket), jsonString, TicketCallback, null);
            return JsonConvert.DeserializeObject<Ticket>(mServer.EndPipeCommand(result));
        }
        public Guid CanceledTicket(Guid id)
        {
            var jsonString = JsonConvert.SerializeObject(id);
            mServer.BeginPipeCommand(nameof(ProcessorCommands.CanceledTicket), jsonString, TicketCallback, null);
            return id;
        }

        private void TicketCallback(IAsyncResult ar)
        {
            var state = ar.AsyncState;
            var end = ar.IsCompleted;
            System.Diagnostics.Debug.WriteLine("ar.IsCompleted = " + ar.IsCompleted);
        }
        public string GetAllProcessingTicketsAsync()
        {
            var result = mServer.BeginPipeCommand(nameof(ProcessorCommands.GetAllProcessingTickets), "[]", TicketCallback, null);
            var str = string.Empty;
            try
            {
                result.AsyncWaitHandle.WaitOne();
                str = mServer.EndPipeCommand(result);
                result.AsyncWaitHandle.Close();
            }
            catch (FaultException ex)
            {
                str = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            result.AsyncWaitHandle.Close();
            return str;
        }
        public string GetAllProcessingTickets()
        {
            try
            {
                return mServer.PipeCommandSync(nameof(ProcessorCommands.GetAllProcessingTickets), "[]");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return string.Empty;

        }
        public string GetAllHistoryTickets()
        {
            try
            {
                return mServer.PipeCommandSync(nameof(ProcessorCommands.GetAllHistoryTickets), "[]");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return string.Empty;
        }
    }
}
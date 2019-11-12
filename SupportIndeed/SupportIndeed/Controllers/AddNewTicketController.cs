using ProcessorIndeed.Models.Documents;
using SupportIndeed.Configuration;
using System;
using System.Web.Http;

namespace SupportIndeed.Controllers
{
    //[Route("api/AddNewTicket")]
    public class AddNewTicketController : ApiController
    {
        public void Post([FromBody]string value)
        {
            ClientProcessor.Instance.InitPipeClient();
            var commander = ClientProcessor.Instance.PClient.JetCommander;
            commander.AddNewTicket(value);
        }
        public void Delete(Guid id)
        {
            ClientProcessor.Instance.InitPipeClient();
            var commander = ClientProcessor.Instance.PClient.JetCommander;
            commander.CanceledTicket(id);
        }

        public IHttpActionResult Get(Guid id)
        {
            ClientProcessor.Instance.InitPipeClient();
            var commander = ClientProcessor.Instance.PClient.JetCommander;
            Ticket ticket= commander.GetTicket(id);
            return Ok(ticket);
        }
    }
}

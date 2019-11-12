using Newtonsoft.Json;
using ProcessorIndeed.CommonData;
using ProcessorIndeed.Models.Documents;
using SupportIndeed.Configuration;
using SupportIndeed.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SupportIndeed.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
        public ActionResult GetTicketsTab()
        {
            ClientProcessor.Instance.InitPipeClient();
            var commander = ClientProcessor.Instance.PClient.JetCommander;
            var current = commander.GetAllProcessingTickets();
            var history = commander.GetAllHistoryTickets();
            var checkCurrent = !string.IsNullOrEmpty(current) && current != LiteralStrings.OK;
            var checkHistory = !string.IsNullOrEmpty(history) && history != LiteralStrings.OK;
            if (!checkCurrent && !checkHistory)
                return View();
            var ticketsContainer = new TicketsContainer()
            {
                HistoryContainer = new History() { Collection = JsonConvert.DeserializeObject<ICollection<Ticket>>(history) },
                CurrentContainer = new Current() { Collection = JsonConvert.DeserializeObject<ICollection<Ticket>>(current) }
            };
            return View(ticketsContainer);
            
        }
        
        [HttpPost]
        public ActionResult AllTickets()
        {
            ClientProcessor.Instance.InitPipeClient();
            var commander = ClientProcessor.Instance.PClient.JetCommander;
            var tickets = commander.GetAllProcessingTickets();
            if (string.IsNullOrEmpty(tickets) || tickets == LiteralStrings.OK)
                return View();
            var current = new Current() { Collection = JsonConvert.DeserializeObject<ICollection<Ticket>>(tickets) };
            return View(current);
        }
        [HttpPost]
        public ActionResult AllHistoryTickets()
        {
            ClientProcessor.Instance.InitPipeClient();
            var commander = ClientProcessor.Instance.PClient.JetCommander;
            var tickets = commander.GetAllHistoryTickets();
            if (string.IsNullOrEmpty(tickets) || tickets == LiteralStrings.OK)
                return View();
            var history = new History() { Collection = JsonConvert.DeserializeObject<ICollection<Ticket>>(tickets) };
            return View(history);
        }

    }
}
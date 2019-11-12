using Newtonsoft.Json;
using ProcessorIndeed.Models.Documents;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TestingWebApi
{
    class Program
    {
        static HttpClient client = new HttpClient();
        private static int port;
        private static int requestCounter;
        private static int ticketProcessingPeriod;
        static void Main()
        {
            var ticketProcessingPeriodConfig = ConfigurationManager.AppSettings["TicketProcessingPeriodSecondes"];
            var requestCounterConfig = ConfigurationManager.AppSettings["RequestCounter"];
            var portConfig = ConfigurationManager.AppSettings["Port"];
            if (!int.TryParse(portConfig, out port))
                port = 59663;
            if (!int.TryParse(requestCounterConfig, out requestCounter))
                requestCounter = 10;
            if (!int.TryParse(ticketProcessingPeriodConfig, out ticketProcessingPeriod))
                ticketProcessingPeriod = 5;
            RunAsync().GetAwaiter().GetResult();
        }

        static void ShowTicket(Ticket ticket)
        {
            Console.WriteLine($"Title: {ticket.Title}\tBody: " +
                $"{ticket.Body}");
        }

        static async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            var ticketStr = JsonConvert.SerializeObject(ticket);
            var response = await client.PostAsJsonAsync(
            "api/AddNewTicket", ticketStr);
            //response.EnsureSuccessStatusCode();
            var ticketS = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Ticket>(ticketS);
            //return response.Headers?.Location;
        }

        static async Task<Ticket> GetTicketAsync(Guid id)
        {
            var response = await client.GetAsync(
                $"api/Tickets/{id}");
            var ticketStr = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Ticket>(ticketStr);
        }
        static async Task<HttpStatusCode> DeleteTicketAsync(Guid id)
        {
            var response = await client.DeleteAsync(
                $"api/Tickets/{id}");
            return response.StatusCode;
        }
        static async Task<Ticket> UpdateTicketAsync(Ticket ticket)
        {
            var ticketStr = JsonConvert.SerializeObject(ticket);
            var response = await client.PutAsJsonAsync(
                $"api/Tickets/{ticket.id}", ticketStr);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated Ticket from the response body.
            ticketStr = await response.Content.ReadAsAsync<string>();
            ticket = JsonConvert.DeserializeObject<Ticket>(ticketStr);
            return ticket;
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri($"http://localhost:{port}/");
            client.DefaultRequestHeaders.Add("ContentType", "application/json;charset=UTF-8");
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var counter = requestCounter;
            try
            {
                do
                {
                    Console.WriteLine($"Create a new Ticket in {DateTime.Now}");
                    var ticket = new Ticket
                    {
                        Title = "Test",
                        Body = "Bla bla bla"
                    };
                    var url = await CreateTicketAsync(ticket);
                    //Console.WriteLine($"Created url at {url?.AbsoluteUri}");
                    //Get the Ticket
                    //ticket = await GetTicketAsync(url?.PathAndQuery);
                    ShowTicket(ticket);
                    // Get the updated Ticket
                    ticket = await GetTicketAsync(ticket.id);
                    ShowTicket(ticket);

                    Thread.Sleep(ticketProcessingPeriod * 1000);
                    counter--;
                } while (counter > 0 || requestCounter == 0);

                //// Update the Ticket
                //Console.WriteLine("Updating price...");
                //ticket.Body = "exchange body";
                //await UpdateTicketAsync(ticket);

                //// Delete the Ticket
                //var statusCode = await DeleteTicketAsync(ticket.id);
                //Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            Console.WriteLine($"Task on stress test completed in {DateTime.Now} , key down Enter pls.");
            Console.ReadLine();
        }
    }
}
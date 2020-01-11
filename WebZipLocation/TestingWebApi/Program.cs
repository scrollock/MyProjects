using ModelZipLocation;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TestingWebApi
{
    class Program
    {
        static HttpClient client = new HttpClient();
        private static string baseUrl;
        private static string zipCode;
        private static int port;

        static void Main(string[] args)
        {
            baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            zipCode = ConfigurationManager.AppSettings["ZipCode"];
            var portConfig = ConfigurationManager.AppSettings["Port"];
            if (args.Length > 0)
            {
                zipCode = args[0];
            }
            if (!int.TryParse(portConfig, out port))
                port = 60070;
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task<Location> GetLocationAsync(Location loc)
        {
            var response = await client.GetAsync($"api/Values/{loc.ZipCode}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result =  JsonConvert.DeserializeObject<Location>(responseBody);
            return result;
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri($"http://{baseUrl}:{port}/");
            client.DefaultRequestHeaders.Add("ContentType", "application/json;charset=UTF-8");
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine($"Create a new Location in {DateTime.Now}");
            var location = new Location
            {
                ZipCode = zipCode
            };
            try
            {
                location = await GetLocationAsync(location);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            Console.WriteLine(location.FrandlyMessage);
            Console.WriteLine($"Task on test completed in {DateTime.Now} , key down Enter pls.");
            Console.ReadLine();
        }
    }
}


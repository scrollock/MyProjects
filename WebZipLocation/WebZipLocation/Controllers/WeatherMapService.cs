using ModelZipLocation;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using WebZipLocation.Models;

namespace WebZipLocation.Controllers
{
    public class WeatherMapService : WebApiBaseService, IWeatherMapService
    {
        private string weatherMapUrl;
        public override void FillInformation(Location location, RequestVerb verb)
        {
            weatherMapUrl = ConfigurationManager.AppSettings["WeatherMapUrl"];
            var lingual = ConfigurationManager.AppSettings["Lingual"];
            var key = ConfigurationManager.AppSettings["WeatherKey"];
            if (string.IsNullOrEmpty(weatherMapUrl) || string.IsNullOrEmpty(lingual))
                location.ErrorMessage += "Settings in config is empty" + StaticConstants.ColoneSpace;
            if(verb == RequestVerb.POST)
                FillInformationPost(location, weatherMapUrl, lingual, key);
            if (verb == RequestVerb.GET)
                FillInformationGet(location, weatherMapUrl, lingual, key);
        }

        private void FillInformationGet(Location location, string url, string lingual, string key)
        {
            var urlGet = $@"{weatherMapUrl}?zip={location.ZipCode},{lingual}&appid={key}";
            var request = WebRequest.Create(urlGet);
            request.Method = nameof(RequestVerb.GET);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                ReadResponse(response, location);
            }
        }
        private void FillInformationPost(Location location, string url, string lingual, string key)
        {
            var request = WebRequest.Create(url);
            request.Method = nameof(RequestVerb.POST);
            var data = string.Format($"zip={location.ZipCode}&{lingual}&{key}");
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.WriteLine(data);
            }
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                ReadResponse(response, location);
            }
        }

        private void ReadResponse(HttpWebResponse response, Location location)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                location.ErrorMessage += $"Response StatusCode is {response.StatusDescription}" + StaticConstants.ColoneSpace;
            }
            try
            {
                using (var receiveStream = response.GetResponseStream())
                using (var readStream = new StreamReader(receiveStream))
                {
                    string str = readStream.ReadToEnd().ToString();
                    var result = JsonConvert.DeserializeObject<WeatherLoc>(str);
                    if (result?.cod == 200)
                    {
                        location.CurrentTemperature = (result.main?.temp ?? 0).ToString();
                        location.Coord = result.coord;
                    }
                    else
                        location.ErrorMessage += result?.message + StaticConstants.ColoneSpace;
                }
            }
            catch (Exception ex)
            {
                location.Exceptions.Add(ex);
                location.ErrorMessage += ex.Message + StaticConstants.ColoneSpace;
            }
        }

        
    }

}
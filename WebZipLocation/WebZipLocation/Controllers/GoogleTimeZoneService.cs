using ModelZipLocation;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using WebZipLocation.Models;

namespace WebZipLocation.Controllers
{
    public class GoogleTimeZoneService : WebApiBaseService, IGoogleTimeZoneService
    {
        private string url;
        private string key;
        public override void FillInformation(Location location, RequestVerb verb)
        {
            url = ConfigurationManager.AppSettings["GoogleTimeZoneUrl"];
            key = string.IsNullOrEmpty(location.Key)? ConfigurationManager.AppSettings["GoogleApiKey"] : location.Key;
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
                location.ErrorMessage += "Settings in config is empty" + StaticConstants.ColoneSpace;
            if(location.Coord == null)
            {
                location.ErrorMessage += "latitude and longitude is invalid" + StaticConstants.ColoneSpace;
                return;
            }
            if (verb == RequestVerb.GET)
                FillInformationGet(location, url);

        }
        private void FillInformationGet(Location location, string url)
        {
            var urlGet = $@"{url}?location={location.Coord.lat.Replace(",", ".")},{location.Coord.lon.Replace(",", ".")}&timestamp=1458000000&key={key}";
            var request = WebRequest.Create(urlGet);
            request.Method = nameof(RequestVerb.GET);
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
                    var result = JsonConvert.DeserializeObject<Timezone>(str);
                    if (result != null)
                    {
                        location.CityName = result.timeZoneId;
                        location.TimeZone = result.timeZoneName;
                    }
                    else
                        location.ErrorMessage += result?.errorMessage + StaticConstants.ColoneSpace;
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
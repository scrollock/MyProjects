using ModelZipLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using WebZipLocation.Areas.HelpPage;

namespace WebZipLocation
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            foreach (Type t in new Type[] { typeof(Location) })
            {
                List<string> propExample = new List<string>();
                foreach (var p in t.GetProperties().Where(x => x.Name != nameof(Location.FrandlyMessage)))
                {
                    propExample.Add($"{p.Name}={HttpUtility.UrlEncode((config.GetHelpPageSampleGenerator().GetSampleObject(p.PropertyType) ?? string.Empty).ToString())}");
                }
                config.SetSampleForType(string.Join(Environment.NewLine, propExample), new MediaTypeHeaderValue("application/x-www-form-urlencoded"), typeof(string));
                config.SetSampleRequest(string.Join("&", propExample), new MediaTypeHeaderValue("text/plain"), "Values", "Post");
                config.SetActualResponseType(t, "Values", "Post");
            }
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}"
            );
            
        }
    }
}

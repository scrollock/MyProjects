using ModelZipLocation;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using WebZipLocation.Models;

namespace WebZipLocation.Controllers
{
    public class ValuesController : ApiController
    {
        [Description("Text documentation post")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] string zipCode)
        {
            if (string.IsNullOrEmpty(zipCode))
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            var service = ObjectsFabric.CreateObject<ILocationService>();
            var location = ObjectsFabric.CreateObject<Location>();
            location.ZipCode = zipCode;
            service.FillInformation(location, RequestVerb.POST);
            var response = Request.CreateResponse(HttpStatusCode.Created, location);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { ZipCode = zipCode }));
            return response;
        }
        [Description("Text documentation get")]
        [HttpGet]
        public HttpResponseMessage Get(string zipCode)
        {
            if (string.IsNullOrEmpty(zipCode))
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            var service = ObjectsFabric.CreateObject<ILocationService>();
            var location = ObjectsFabric.CreateObject<Location>();
            location.ZipCode = zipCode;
            service.FillInformation(location, RequestVerb.GET);
            return Request.CreateResponse(HttpStatusCode.OK, location, JsonMediaTypeFormatter.DefaultMediaType);
        }
    }
}

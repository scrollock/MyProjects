using ModelZipLocation;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace WebZipLocation.Controllers
{
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        [Description("Text documentation post")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]string zipCode, string key)
        {
            if (string.IsNullOrEmpty(zipCode))
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            var service = ObjectsFabric.CreateObject<ILocationService>();
            var location = ObjectsFabric.CreateObject<Location>();
            location.ZipCode = zipCode;
            location.Key = key;
            service.FillInformation(location, RequestVerb.POST);
            var response = Request.CreateResponse(HttpStatusCode.Created, location);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { ZipCode = zipCode, Key = key}));
            return response;
        }
        [Description("Text documentation get")]
        [HttpGet]
        [Route("{zipCode}/{key}")]
        public HttpResponseMessage Get(string zipCode, string key)
        {
            if (string.IsNullOrEmpty(zipCode))
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            var service = ObjectsFabric.CreateObject<ILocationService>();
            var location = ObjectsFabric.CreateObject<Location>();
            location.ZipCode = zipCode;
            location.Key = key;
            service.FillInformation(location, RequestVerb.GET);
            return Request.CreateResponse(HttpStatusCode.OK, location, JsonMediaTypeFormatter.DefaultMediaType);
        }
    }
}

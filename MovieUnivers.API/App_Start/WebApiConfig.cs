using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using MovieUniverse.API.Filters;
using Newtonsoft.Json;

namespace MovieUniverse.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new GlobalExceptionFilter());
            config.Filters.Add(new ValidationFilterAttribute());
            // Web API configuration and services

            // Web API routes
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors();

            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            

            config.Formatters.Clear();

            config.Formatters.Add(new JsonMediaTypeFormatter());
            
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
        }
    }
}

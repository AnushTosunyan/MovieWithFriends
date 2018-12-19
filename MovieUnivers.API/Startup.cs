using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using MovieUniverse.API.Ninject;
using Ninject;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using MovieUniverse.API.Filters;

[assembly: OwinStartup(typeof(MovieUniverse.API.Startup))]

namespace MovieUniverse.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            
            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            
            app.UseWebApi(config);

            HubConfiguration hubConfig = new HubConfiguration()
            {
                Resolver = GlobalHost.DependencyResolver,
                EnableJavaScriptProxies = true,
                EnableJSONP = true,
                
            };
            

            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = false,
                    EnableJSONP = true,
                    
                };
                
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
            //var autorizationAttribute = new SignalrCustomAuthorizeAttribute();
            //GlobalHost.HubPipeline.AddModule(new AuthorizeModule(autorizationAttribute,
            //    autorizationAttribute));
            //GlobalHost.Configuration.ConnectionTimeout = TimeSpan.MaxValue;
            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                //Provider = NinjectHelper.Kernel.Get<SimpleAuthorizationServerProvider>(),
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}

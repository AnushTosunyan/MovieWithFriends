using System;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using MovieUniverse.Abstract.Config.Ninject;
using MovieUniverse.API;
using MovieUniverse.DAL.NinjectModuls;
using MovieUniverse.Facade.NinjectModule;
using MovieUniverse.Services.NinjectModules;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.WebApi.DependencyResolver;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace MovieUniverse.API
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            
            var kernel = new StandardKernel(new NinjectSettings { InjectNonPublic = true, },
                               new INinjectModule[] { new EmptyModule(),new DataAccessModul(), new ServiceModule(), new AppUserModule(), new FacadeModule(), });
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                GlobalHost.DependencyResolver = new Microsoft.AspNet.SignalR.Ninject.NinjectDependencyResolver(kernel);
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            
            //kernel.Bind<IDbContext>().To<MovieUniverseContext>().InRequestScope();
        }       
    }
}

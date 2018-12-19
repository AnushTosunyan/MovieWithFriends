using MovieUniverse.Abstract.Config.Ninject;
using MovieUniverse.DAL.NinjectModuls;
using MovieUniverse.Facade.NinjectModule;
using MovieUniverse.Services.NinjectModules;
using Ninject;
using Ninject.Modules;

namespace MovieUniverse.API.Ninject
{
    public class NinjectHelper
    {
        private static IKernel _kernel;
        public static IKernel Kernel
        {
            get
            {
                return _kernel ??
                       (_kernel =
                           new StandardKernel(new NinjectSettings {InjectNonPublic = true,},
                               new INinjectModule[] {new DataAccessModul(), new ServiceModule(), new AppUserModule(),new FacadeModule(), }));
            }
        }

    }
}
using Ninject;
using Ninject.Web.Common;

namespace MovieUniverse.API
{
    public class EmptyModule: GlobalKernelRegistrationModule<OnePerRequestHttpModule>
    {
         
    }
}
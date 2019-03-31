using Microsoft.Owin.Logging;
using System.Web.Http.Dependencies;
using Unity;
using Unity.WebApi;

namespace SchoolReportApi
{
    public static class UnityConfig
    {
        public static IDependencyResolver CreateDependencyResolver(ILogger logger)
        {
            var container = new UnityContainer();
            _RegisterComponents(container, logger);

            return new UnityDependencyResolver(container);
        }

        public static void _RegisterComponents(UnityContainer container, ILogger logger)
        {
            container.RegisterInstance<ILogger>(logger);
        }
    }
}
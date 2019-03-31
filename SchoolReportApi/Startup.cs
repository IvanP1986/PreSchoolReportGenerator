using Microsoft.Owin.Logging;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SchoolReportApi
{
    public class Startup
    {
        public void Configuration (IAppBuilder app)
        {
            ILogger logger = app.CreateLogger("SchoolReportApi");
            var config = new HttpConfiguration()
            {
                DependencyResolver = UnityConfig.CreateDependencyResolver(logger)
            };
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}

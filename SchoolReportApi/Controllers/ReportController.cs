using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SchoolReportApi.Controllers
{
    public class ReportController : ApiController
    {
        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "test";
        }
    }
}

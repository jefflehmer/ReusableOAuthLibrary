using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Test_Resource_Server.Controllers
{
    public class TimeApiController : BaseApiController
    {
        //[Authorize]
        //[Authorize(Roles= "SuperUser")]
        //[GatekeeperAuthorize(ClaimType = "MYID", ClaimValue = "123456789")]
        [Route("api/now")]
        [HttpGet]
        public string Current()
        {
            return DateTime.Now.ToString();
        }
    }
}

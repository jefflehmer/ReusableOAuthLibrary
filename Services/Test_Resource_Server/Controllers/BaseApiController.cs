using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Test_Resource_Server.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
        {
            SetupDatabaseConnections();
        }

        protected virtual void SetupDatabaseConnections()
        {
        }
    }
}

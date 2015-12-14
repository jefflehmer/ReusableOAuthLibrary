using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OAuth_Library
{
    public static class ExtensionMethods
    {
        public static void EnableCors(this HttpConfiguration config, string origins, string headers, string methods)
        {
            // by putting this in here we only need to nuget Microsoft.AspNet.WebApi.Cors here and not force it in the resource servers
            config.EnableCors(new EnableCorsAttribute(origins, headers, methods));
        }
    }
}

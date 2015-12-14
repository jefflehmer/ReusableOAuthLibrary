using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using OAuth_Server.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OAuth_Server.Models
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private UserUserManager _AppUserManager;

        public ModelFactory(HttpRequestMessage request, UserUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserUserReturnModel Create(UserUser appUser)
        {
            return new UserUserReturnModel
            {
                MYID = appUser.MYID,
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
            };
        }

        public UserRoleReturnModel Create(IdentityRole appRole)
        {

            return new UserRoleReturnModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };

        }
    }

    public class UserUserReturnModel
    {
        public int MYID { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }

    public class UserRoleReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
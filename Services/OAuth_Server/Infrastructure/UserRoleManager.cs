using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace OAuth_Server.Infrastructure
{
    public class UserRoleManager : RoleManager<IdentityRole>
    {
        public UserRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static UserRoleManager Create(IdentityFactoryOptions<UserRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new UserRoleManager(new RoleStore<IdentityRole>(context.Get<UserDbContext>()));

            return appRoleManager;
        }
    }
}
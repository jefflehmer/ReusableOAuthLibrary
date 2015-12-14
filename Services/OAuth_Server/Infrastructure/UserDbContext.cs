using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OAuth_Server.Infrastructure
{
    public class UserDbContext : IdentityDbContext<UserUser>
    {
        public UserDbContext()
            : base("UserDbContext", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static UserDbContext Create()
        {
            return new UserDbContext();
        }

    }
}
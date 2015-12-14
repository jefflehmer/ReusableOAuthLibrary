using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OAuth_Server.Infrastructure
{
    // this is serialized in the db as the AspNetUsers table from Identity.
    public class UserUser : IdentityUser
    {
        // public string UserName { get; set; } already in base class

        [Required]
        public int MYID { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserUserManager userManager, string authenticationType)
        {
            var userIdentity = await userManager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here or right after this call
            return userIdentity;
        }
    }
}
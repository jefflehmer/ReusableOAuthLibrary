using OAuth_Server.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OAuth_Server.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OAuth_Server.Infrastructure.UserDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OAuth_Server.Infrastructure.UserDbContext context)
        {
            string SeedName = "marcus.cole@xyz.com";
            var user = new UserUser
            {
                 UserName = SeedName
                ,Email = SeedName
                ,EmailConfirmed = true
            };

            var manager = new UserUserManager(new UserStore<UserUser>(context));
            manager.Create(user, "password");// user.Password);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new UserDbContext()));
            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "SuperUser" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByName(SeedName);
            manager.AddToRoles(adminUser.Id, new string[] { "SuperUser", "Admin" });
        }
    }
}

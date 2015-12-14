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
    public class UserUserManager : UserManager<UserUser>
    {
        public UserUserManager(IUserStore<UserUser> store)
            : base(store)
        {
        }

        public static UserUserManager Create(IdentityFactoryOptions<UserUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<UserDbContext>();
            var appUserManager = new UserUserManager(new UserStore<UserUser>(appDbContext));

            // Configure validation logic for usernames
            //appUserManager.UserValidator = new UserValidator<UserUser>(appUserManager)
            //{
            //    AllowOnlyAlphanumericUserNames = true,
            //    RequireUniqueEmail = true
            //};

            //// Configure validation logic for passwords
            //appUserManager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = false,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};

            appUserManager.EmailService = new OAuth_Server.Services.EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<UserUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }


            return appUserManager;
        }
    }
}
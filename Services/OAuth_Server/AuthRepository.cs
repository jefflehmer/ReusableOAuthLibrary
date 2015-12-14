using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth_Server.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OAuth_Server
{
    public class AuthRepository : IDisposable
    {
        private UserDbContext _ctx;

        private UserUserManager _userManager;

        public AuthRepository()
        {
            _ctx = new UserDbContext();
            _userManager = new UserUserManager(new UserStore<UserUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            return result;
        }

        public async Task<UserUser> FindUser(string userName, string password)
        {
            UserUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}

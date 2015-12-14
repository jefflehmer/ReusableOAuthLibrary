using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OAuth_Server.Infrastructure;
using OAuth_Server.Models;
using Microsoft.AspNet.Identity;

namespace OAuth_Server.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        //[Authorize]
        //[Authorize(Roles = "Admin")]
        [ClaimsAuthorization(ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", ClaimValue = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return null;// Created(locationHeader, TheModelFactory.Create(newUser));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("confirm")]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirmUserBindingModel confirmUserBindingModel)
        {
            if (string.IsNullOrWhiteSpace(confirmUserBindingModel.UserId) || string.IsNullOrWhiteSpace(confirmUserBindingModel.Code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            // confirm the password belongs to user with given Id
            IdentityResult result = null;
            var user = await AppUserManager.FindByIdAsync(confirmUserBindingModel.UserId);
            var code = System.Web.HttpUtility.UrlDecode(confirmUserBindingModel.Code).Replace(" ", "+"); // asp.net generates codes with unsafe character, '+' (see 'Duque' comments)
            if (AppUserManager.CheckPassword(user, confirmUserBindingModel.Password))
                result = await this.AppUserManager.ConfirmEmailAsync(confirmUserBindingModel.UserId, code);

            if ((result != null) && result.Succeeded)
            {
                return Ok();
            }

            return GetErrorResult(result);
        }

        /*
        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }
        */

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }
    }
}

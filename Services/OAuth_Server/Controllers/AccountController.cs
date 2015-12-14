using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DD_OAuth_Server.Infrastructure;
using DD_OAuth_Server.Models;
using Microsoft.AspNet.Identity;

namespace DD_OAuth_Server.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;

        public AccountController()
        {
            _repo = new AuthRepository();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(CreateUserBindingModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newPractitioner = new PractitionerUser()
            {
                UserName = userModel.Email, // our usernames are the user email

                DDID = 90521458, // TODO: get from Practitioner table for now?
                //Resource = "PAT",
                //Role = "User",

                Email = userModel.Email,
                NPI = userModel.NPI,
                Degree = userModel.Degree,
                Specialty = userModel.Specialty,
                Password = userModel.Password
            };

            var result = await _repo.RegisterUser(newPractitioner, userModel.Password);
            var errorResult = GetErrorResult(result);

            return (errorResult != null) ? errorResult : Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}

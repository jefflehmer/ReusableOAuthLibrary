using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using OAuth_Server.Formats;
using OAuth_Server.Infrastructure;
using OAuth_Server.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(OAuth_Server.Startup))]
namespace OAuth_Server
{
    public class Startup
    {
        public const string This_Server_Http = "http://localhost:61451";

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(UserDbContext.Create);
            app.CreatePerOwinContext<UserUserManager>(UserUserManager.Create);
            app.CreatePerOwinContext<UserRoleManager>(UserRoleManager.Create);

            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                 AllowInsecureHttp = true
                ,TokenEndpointPath = new PathString("/oauth/token")
                ,AccessTokenExpireTimeSpan = TimeSpan.FromDays(1)//.FromMinutes(30)
                ,Provider = new CustomOAuthProvider()
                ,AccessTokenFormat = new CustomJwtFormat(This_Server_Http)
                //,RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = This_Server_Http;

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, "xyz")
                    }
                });
        }
    }
}
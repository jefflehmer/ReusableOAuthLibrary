# ReusableOAuthLibrary
Steps on how to include the new OAuth_Library in a separate Resource Server that will support the OAuth Claim Attributes for Security.  This code is a simple extension from the lessons learned on Taiseer Joudeh's blog on adding OAuth to your ASP.NET Web API project using OWIN and Identity.  Taiseer's blog can be found at this link.  This code will make a lot more sense if you've read it.

http://bitoftech.net/2015/03/31/asp-net-web-api-claims-authorization-with-asp-net-identity-2-1/

Steps to enable an independent DD Resource Server that supports the new DD OAuth Claim Attributes for Security

In the DD Resource Server project (C#):

1. ONE Add a reference to the “DD_OAuth_Library” in your resource project.

2. TWO You need to reference the OWIN services defined in the DD_OAuth_Library  in your resource server.

In your web.config, make sure you have the following appSetting value

      <appSettings>
        <add key="owin:appStartup" value="DD_OAuth_Library.Startup, DD_OAuth_Library" />
      </appSettings>
      
3. THREE Make sure you are not inadvertently removing the “OptionsVerbHandler” from your web server.  This will allow the browser to send a “preflight” request to your web service to verify it can handle a CORS request.

In your web.config file and add the following.

      <system.webServer>
        <handlers>
          <remove name="OPTIONSVerbHandler" /> <!-- keep this one in  -->
          <add name="OPTIONSVerbHandler" path="*" verb="OPTIONS" modules="ProtocolSupportModule" resourceType="Unspecified" requireAccess="None" />
          
4.  FOUR Set the following in your WebApiConfig.Register(..) method: 

        using DD_OAuth_Library;
        config.EnableCors("*", "*", "*");

Using the Authorization Attributes to enforce security:

1.	There are two possible security attributes you can now use before either your ApiController declaration or the separate api methods.

a.	[Authorize] Only cares that the user has been authorized somehow.

b.	[Authorize(Roles = "Practitioner")]  User must be authorized and have this “role”.

In the SPA pages that use Angular v1.x:

1.	Angular needs to know it will be working with a resource server with a different origin.  To CORS-enable the Angular module you will need to add the following to the app.config:

        app.config(['$httpProvider', function ($httpProvider) {
            $httpProvider.defaults.useXDomain = true; // this is the new line
            $httpProvider.interceptors.push('authInterceptorService');
        }]);
        
2.	You can make calls to the new server now. E.g., 

        $http.get("http://localhost:24483/api/now")
            .success(function (response) { $scope.message = response; });
            
Note: If you started with an “Empty” AspNet project you may need to add the following nuget packages. 

There are a lot you won’t have to worry about because they were moved to the DD_OAuth_Library.

1.	Microsoft.AspNet.WebApi.Client
2.	Microsoft.AspNet.WebApi.Core
3.	Microsoft.AspNet.WebApi.WebHost
4.	Microsoft.Owin
5.	Owin

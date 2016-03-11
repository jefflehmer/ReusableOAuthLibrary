# ReusableOAuthLibrary
Steps on how to include the new OAuth_Library in a separate Resource Server that will support the OAuth Claim Attributes for Security.  This code is a simple extension from the lessons learned on Taiseer Joudeh's blog on adding OAuth to your ASP.NET Web API project using OWIN and Identity.  Use this url to test this project.  It simply returns the time and puts it into a message immediately below the button.

http://localhost:19985/test.html#/testserver

Taiseer's blog can be found at this link.  This code will make a lot more sense if you've read it.

http://bitoftech.net/2015/03/31/asp-net-web-api-claims-authorization-with-asp-net-identity-2-1/

Steps to enable an independent Resource Server that supports the new OAuth Claim Attributes for Security

In the Resource Server project (C#):

1. ONE Add a reference to the “OAuth_Library” in your resource project.

2. TWO You need to reference the OWIN services defined in the OAuth_Library  in your resource server.

In your web.config, make sure you have the following appSetting value

      <appSettings>
        <add key="owin:appStartup" value="OAuth_Library.Startup, OAuth_Library" />
      </appSettings>
      
3. THREE Make sure you are not inadvertently removing the “OptionsVerbHandler” from your web server.  This will allow the browser to send a “preflight” request to your web service to verify it can handle a CORS request.

In your web.config file and add the following.

      <system.webServer>
        <handlers>
          <remove name="OPTIONSVerbHandler" /> <!-- keep this one in  -->
          <add name="OPTIONSVerbHandler" path="*" verb="OPTIONS" modules="ProtocolSupportModule" resourceType="Unspecified" requireAccess="None" />
          
4.  FOUR Set the following in your WebApiConfig.Register(..) method: 

        using OAuth_Library;
        config.EnableCors("*", "*", "*");


Note: If you started with an “Empty” AspNet project you may need to add the following nuget packages. 
There are a lot you won’t have to worry about because they were moved to the OAuth_Library.
1.	Microsoft.AspNet.WebApi.Client
2.	Microsoft.AspNet.WebApi.Core
3.	Microsoft.AspNet.WebApi.WebHost
4.	Microsoft.Owin.Host.SystemWeb !!! REMEMBER THIS ONE!  If your resource server works locally but not deployed to the server it could be because you aren't including this.  It will work locally because the VS project includes it but VS will not "Publish" it to the server.
5.	Microsoft.Owin (may not be necessary for your project)
6.	Owin (may not be necessary for your project)


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
            

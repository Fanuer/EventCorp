
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using EventCorp.Clients;
using EventCorp.EventServer.Interfaces;
using EventCorp.EventServer.Repositories;
using EventCorps.Helper;
using EventCorps.Helper.HttpAccess;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace EventCorp.EventServer
{
  public class Startup
  {
    /*
    NUGET Setup:

      Install-Package Microsoft.AspNet.WebApi -Version 5.2.2
      Install-Package Microsoft.AspNet.WebApi.Owin
      Install-Package Microsoft.Owin.Host.SystemWeb -Version 3.0.0
      Install-Package Microsoft.Owin.Cors -Version 3.0.0
      Install-Package Microsoft.Owin.Security.Jwt -Version 3.0.0
      Install-Package Microsoft.AspNet.Identity.Owin
      Install-Package Swashbuckle.Core

      Install-Package EntityFramework
      */

    public void Configuration(IAppBuilder app)
    {
      var config = new HttpConfiguration();
      config.MapHttpAttributeRoutes();
      ConfigureOAuthTokenConsumption(app);
      app.UseCors(CorsOptions.AllowAll);
      Utilities.InitialiseSwagger(config, "EventCorp Event API", Assembly.GetExecutingAssembly().GetName().Name);
      SetOwinVariables(app);
      app.UseWebApi(config);
    }

    private void SetOwinVariables(IAppBuilder app)
    {
      app.CreatePerOwinContext(EventContext.Create);
      app.CreatePerOwinContext(EventContext.CreateRepository);
      app.CreatePerOwinContext(SimpleManagementSession.Create);
    }

    /// <summary>
    /// Configures how the web api should handle authorization.
    /// The Api will now only trust issues by our Authorization Server and if Authorization Server = Resource Server
    /// </summary>
    /// <param name="app"></param>
    private void ConfigureOAuthTokenConsumption(IAppBuilder app)
    {
      var issuer = ConfigurationManager.AppSettings["as:Issuer"];

      // Api controllers with an [Authorize] attribute will be validated with JWT
      app.UseJwtBearerAuthentication(
          new JwtBearerAuthenticationOptions
          {
            AuthenticationMode = AuthenticationMode.Active,
            AllowedAudiences = Const.Audiences.Keys.ToArray(),
            IssuerSecurityTokenProviders = Const.Audiences.Values.Select(x => new SymmetricKeyIssuerSecurityTokenProvider(issuer, TextEncodings.Base64Url.Decode(x))).ToArray()
          });
    }

  }
}
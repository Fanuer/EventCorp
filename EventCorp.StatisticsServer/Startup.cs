using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using EventCorp.Clients;
using EventCorps.Helper;
using EventCorps.Helper.HttpAccess;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace EventCorp.StatisticsServer
{
  public class Startup
  {

    public void Configuration(IAppBuilder app)
    {
      var config = new HttpConfiguration();
      config.MapHttpAttributeRoutes();
      ConfigureOAuthTokenConsumption(app);
      app.UseCors(CorsOptions.AllowAll);
      Utilities.InitialiseSwagger(config, "EventCorp Statistics API", Assembly.GetExecutingAssembly().GetName().Name);
      app.UseWebApi(config);
    }

    /// <summary>
    /// Configures how the web api should handle authorization.
    /// The Api will now only trust issues by our Authorization Server and if Authorization Server = Resource Server
    /// </summary>
    /// <param name="app"></param>
    private void ConfigureOAuthTokenConsumption(IAppBuilder app)
    {
      app.CreatePerOwinContext(SimpleManagementSession.Create);

      var issuer = Const.Issuer;

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
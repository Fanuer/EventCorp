﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using EventCorp.Clients;
using EventCorps.Helper;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace EventCorp.Recommendation
{
  public class Startup
  {
    private readonly Dictionary<string, string> _audiences = new Dictionary<string, string>()
        {
            {"0dd23c1d3ea848a2943fa8a250e0b2ad",  "Uy6qvZV0iA2/drm4zACDLCCm7BE9aCKZVQ16bg80XiU="}, //Test
            {"4d874d92589f4357ba19a2b91e0be5ff",  "PUP3EI8G137YIW1TnoCVrwhu0KnpYYQHNWJcM9UU+mI="}, //Rec
            {"b9f6fba178aa4081a010f3b7a2c91ded",  "5U06jgp+GLxa32YnPjZSvfxL8QQjPmKtyELnygljJN4="}, //Auth
        };

    public void Configuration(IAppBuilder app)
    {
      var config = new HttpConfiguration();
      config.MapHttpAttributeRoutes();
      ConfigureOAuthTokenConsumption(app);
      app.UseCors(CorsOptions.AllowAll);
      Utilities.InitialiseSwagger(config, "EventCorp Recommendation API", Assembly.GetExecutingAssembly().GetName().Name);
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

      var issuer = ConfigurationManager.AppSettings["as:Issuer"];

      // Api controllers with an [Authorize] attribute will be validated with JWT
      app.UseJwtBearerAuthentication(
          new JwtBearerAuthenticationOptions
          {
            AuthenticationMode = AuthenticationMode.Active,
            AllowedAudiences = _audiences.Keys.ToArray(),
            IssuerSecurityTokenProviders = _audiences.Values.Select(x => new SymmetricKeyIssuerSecurityTokenProvider(issuer, TextEncodings.Base64Url.Decode(x))).ToArray()
          });
    }

  }
}
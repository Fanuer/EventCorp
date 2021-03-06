﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using EventCorp.AuthorizationServer.Formats;
using EventCorp.AuthorizationServer.Manager;
using EventCorp.AuthorizationServer.Providers;
using EventCorp.AuthorizationServer.Repository;
using EventCorps.Helper;
using EventCorps.Helper.Filter;
using EventCorps.Helper.HttpAccess;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof(EventCorp.AuthorizationServer.Startup))]
namespace EventCorp.AuthorizationServer
{
    public class Startup
    {
        /*
  {  
     "id":"0dd23c1d3ea848a2943fa8a250e0b2ad",
     "secret":"Uy6qvZV0iA2/drm4zACDLCCm7BE9aCKZVQ16bg80XiU=",
     "name":"Test",
     "active":false,
     "allowedOrigin":null
  }       */

        #region Methods
        /// <summary>
        /// Get's fired when the applications is started by the host
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);
            ConfigureWebApi(httpConfig);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            Utilities.InitialiseSwagger(httpConfig, "EventCorp AuthServer API", Assembly.GetExecutingAssembly().GetName().Name);
            app.UseWebApi(httpConfig);
        }

        /// <summary>
        /// Configure the db context and user manager to use a single instance per request
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            app.CreatePerOwinContext(AuthContext.Create);
            app.CreatePerOwinContext(AuthContext.CreateRepository);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
#warning For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CustomOAuthProvider(), // specify, how to validate the Resource Owner
                AccessTokenFormat = new CustomJwtFormat(ConfigurationManager.AppSettings["as:Issuer"]), //Specifies the implementation, how to generate the access token
                RefreshTokenProvider = new CustomRefreshTokenProvider()
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        /// <summary>
        /// Defines Web Api Routing and Json-Response-Resolver 
        /// </summary>
        /// <param name="config"></param>
        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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

        #endregion
    }
}

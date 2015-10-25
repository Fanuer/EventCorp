using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using EventCorp.AuthorizationServer.Formats;
using EventCorp.AuthorizationServer.Manager;
using EventCorp.AuthorizationServer.Providers;
using EventCorp.AuthorizationServer.Repository;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof(EventCorp.AuthorizationServer.Startup))]
namespace EventCorp.AuthorizationServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            this.ConfigureWebApi(config);
            this.ConfigureOAuthTokenGeneration(app);
            this.InitialiseSwagger(config);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
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
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        /// <summary>
        /// Initializes Swagger Documentation
        /// </summary>
        /// <param name="httpConfig"></param>
        private void InitialiseSwagger(HttpConfiguration httpConfig)
        {
            try
            {
                httpConfig
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "fIT Api");
                    c.IncludeXmlComments(GetXmlPath());
                    c.IgnoreObsoleteActions();
                    c.DescribeAllEnumsAsStrings();
                    c.OAuth2("oauth2")
            .Description("OAuth2 Password Grant")
            .Flow("password")
            //.AuthorizationUrl("http://petstore.swagger.wordnik.com/api/oauth/dialog")
            .TokenUrl("/Accounts/Login")
            .Scopes(scopes =>
                  {
                      scopes.Add("user", "Read access to protected resources");
                      scopes.Add("admin", "Write access to protected resources");
                  });
                })
                .EnableSwaggerUi(c =>
                {
                    c.InjectJavaScript(typeof(Startup).Assembly, "fIT.WebApi/js/onComplete.js");
                });
            }
            catch (Exception e)
            {
                throw new Exception("Error on creating Swagger: " + e.Message + Environment.NewLine + e.StackTrace);
            }

        }

        private static string GetXmlPath()
        {
            return Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "bin", "EventCorp.AuthorizationServer.XML");

        }

    }
}

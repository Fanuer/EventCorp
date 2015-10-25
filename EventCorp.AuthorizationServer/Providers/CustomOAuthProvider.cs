using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Interfaces.Repositories;
using EventCorp.AuthorizationServer.Manager;
using EventCorp.AuthorizationServer.Repository;
using EventCorps.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace EventCorp.AuthorizationServer.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// responsible for validating if the Resource server (audience) is already registered in our Authorization server by reading the client_id value from the request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
            }
            if (!context.HasError)
            {
                var audience = AudiencesStore.Instance.FindAudience(context.ClientId);
                if (audience == null)
                {
                    context.SetError("invalid_clientId", $"Client '{context.ClientId}' is not registered in the system.");
                }
                else if (audience.Secret != Utilities.GetHash(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret is invalid.");
                }
                else if (!audience.Active)
                {
                    context.SetError("invalid_clientId", "Client is inactive.");
                }
                else
                {
                    context.OwinContext.Set("as:clientId", clientId);
                    context.OwinContext.Set("as:clientAllowedOrigin", audience.AllowedOrigin);
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// validating the resource owner (user) credentials
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //Search user by username and password
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            User user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            using (IApplicationRepository repo = new ApplicationRepository())
            {
                var oldtokens = (await repo.RefreshTokens.GetAllAsync()).Where(x => x.ExpiresUtc < DateTime.UtcNow || x.Subject.Equals(user.UserName)).ToList();
                foreach (var token in oldtokens)
                {
                    await repo.RefreshTokens.RemoveAsync(token);
                }
            }

            var clientId = context.OwinContext.Get<string>("as:clientId");
            var identity = await user.GenerateUserIdentityAsync(userManager, "JWT", clientId);

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                         "audience", context.ClientId ?? string.Empty
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            if (!context.AdditionalResponseParameters.ContainsKey("UserId"))
            {
                context.AdditionalResponseParameters.Add("UserId", context.Identity.GetUserId());
            }
            else
            {
                context.AdditionalResponseParameters["UserId"] = context.Identity.GetUserId();
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary.ContainsKey("as:client_id") ? context.Ticket.Properties.Dictionary["as:client_id"] : "";
            var currentClient = context.ClientId ?? "";

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }


    }
}

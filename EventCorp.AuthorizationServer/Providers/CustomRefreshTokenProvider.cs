using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Interfaces.Repositories;
using EventCorp.AuthorizationServer.Repository;
using EventCorps.Helper;
using Microsoft.Owin.Security.Infrastructure;

namespace EventCorp.AuthorizationServer.Providers
{
    public class CustomRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private const string IS_REFREHTOKEN_EXPIRED_NAME = "IsRefreshTokenExpired";

        public void Create(AuthenticationTokenCreateContext context)
        {
            this.CreateAsync(context).Wait();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            if (!context.OwinContext.Environment.ContainsKey(IS_REFREHTOKEN_EXPIRED_NAME) || (bool)context.OwinContext.Environment[IS_REFREHTOKEN_EXPIRED_NAME])
            {
                bool result = false;
                var refreshTokenId = Guid.NewGuid().ToString("n");
                var clientId = context.Ticket.Properties.Dictionary["audience"];

                var refreshTokenLifetime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime") ?? "30";
                var token = new RefreshToken()
                {
                    Id = Utilities.GetHash(refreshTokenId),
                    ClientId = clientId,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddDays(Double.Parse(refreshTokenLifetime))
                };
                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                using (IApplicationRepository rep = new ApplicationRepository())
                {
                    result = await rep.RefreshTokens.AddAsync(token);
                }
                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            this.ReceiveAsync(context).Wait();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            try
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                var hashedTokenId = Utilities.GetHash(context.Token);
                using (IApplicationRepository rep = new ApplicationRepository())
                {
                    var refreshToken = await rep.RefreshTokens.FindAsync(hashedTokenId);

                    if (refreshToken != null)
                    {
                        //Get protectedTicket from refreshToken class
                        context.DeserializeTicket(refreshToken.ProtectedTicket);
                        var result = await rep.RefreshTokens.RemoveAsync(hashedTokenId);
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.AuthorizationServer.Interfaces.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventCorp.AuthorizationServer.Repository
{
    public class ApplicationRepository:IApplicationRepository
    {
        #region Field
        private readonly AuthContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;
        #endregion

        #region Ctor

        public ApplicationRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            RefreshTokens = new RefreshTokenRepository(_ctx);
            Audiences = new AudienceRepository(_ctx);
        }
        #endregion

        #region Methods

        void IDisposable.Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
        #endregion

        #region Properties
        public IAudienceRepository Audiences { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        #endregion
    }
}

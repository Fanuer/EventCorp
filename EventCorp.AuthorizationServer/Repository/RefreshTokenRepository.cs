using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Interfaces.Repositories;
using EventCorps.Helper.DBAccess;

namespace EventCorp.AuthorizationServer.Repository
{
    internal class RefreshTokenRepository : GenericRepository<RefreshToken, string>, IRefreshTokenRepository
    {
        #region Ctor

        public RefreshTokenRepository(AuthContext ctx) : base(ctx) { }
        #endregion
        #region Methods

        #endregion
    }
}
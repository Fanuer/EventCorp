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
        public override async Task<IQueryable<RefreshToken>> GetAllAsync()
        {
            var result = new List<RefreshToken>();
            var context = this.DBContext as AuthContext;

            if (context != null)
            {
                result = await context.RefreshTokens.ToListAsync();
                return result.AsQueryable();
            }
            return result.AsQueryable();
        }

        public override async Task<RefreshToken> FindAsync(string id)
        {
            RefreshToken result = null;
            var context = this.DBContext as AuthContext;
            if (context != null)
            {
                result = await context.RefreshTokens.FindAsync(id);
            }
            return result;
        }

        #endregion
    }
}
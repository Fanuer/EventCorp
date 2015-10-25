using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Interfaces.Repositories;
using EventCorps.Helper.DBAccess;

namespace EventCorp.AuthorizationServer.Repository
{
    public class AudienceRepository : GenericRepository<Audience, string>, IAudienceRepository
    {

        #region Ctor

        public AudienceRepository(AuthContext ctx):base(ctx)
        {
        }
        #endregion

        #region Methods
        public override async Task<Audience> FindAsync(string id)
        {
            Audience result = null;
            var context = this.DBContext as AuthContext;
            if (context != null)
            {
                result = await context.Audiences.FindAsync(id);
            }
            return result;
        }

        public override async Task<IQueryable<Audience>> GetAllAsync()
        {
            var result = new List<Audience>();
            var context = this.DBContext as AuthContext;

            if (context != null)
            {
                result = await context.Audiences.ToListAsync();
                return result.AsQueryable();
            }
            return result.AsQueryable();

        }
        #endregion
    }
}
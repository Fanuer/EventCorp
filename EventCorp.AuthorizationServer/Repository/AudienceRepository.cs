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
        #endregion
    }
}
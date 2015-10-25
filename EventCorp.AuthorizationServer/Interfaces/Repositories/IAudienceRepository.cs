using EventCorp.AuthorizationServer.Entites;
using EventCorps.Helper.DBAccess.Interfaces;
using EventCorps.Helper.DBAccess.Interfaces.SingleId;

namespace EventCorp.AuthorizationServer.Interfaces.Repositories
{
    public interface IAudienceRepository : IRepositoryFindSingle<Audience, string>, IRepositoryAddAndDelete<Audience, string>, IRepositoryFindAll<Audience>
    {
         
    }
}
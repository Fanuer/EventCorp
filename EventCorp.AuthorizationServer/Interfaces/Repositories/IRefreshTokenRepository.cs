using EventCorp.AuthorizationServer.Entites;
using EventCorps.Helper.DBAccess.Interfaces;
using EventCorps.Helper.DBAccess.Interfaces.SingleId;

namespace EventCorp.AuthorizationServer.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IRepositoryAddAndDelete<RefreshToken, string>, IRepositoryFindAll<RefreshToken>, IRepositoryFindSingle<RefreshToken, string>
    {
    }
}

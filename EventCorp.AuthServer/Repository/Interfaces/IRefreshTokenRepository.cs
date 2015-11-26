using EventCorp.AuthServer.Entities;
using EventCorp.AuthServer.Repository.Interfaces.CRUD;
using EventCorp.AuthServer.Repository.Interfaces.CRUD.SingleID;

namespace EventCorp.AuthServer.Repository.Interfaces
{
    internal interface IRefreshTokenRepository : IRepositoryAddAndDelete<RefreshToken, string>, IRepositoryFindAll<RefreshToken>, IRepositoryFindSingle<RefreshToken, string>
    {
  }
}
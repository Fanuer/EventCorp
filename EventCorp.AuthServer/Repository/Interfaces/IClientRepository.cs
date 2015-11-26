using EventCorp.AuthServer.Entities;
using EventCorp.AuthServer.Repository.Interfaces.CRUD.SingleID;

namespace EventCorp.AuthServer.Repository.Interfaces
{
    internal interface IClientRepository : IRepositoryFindSingle<Client, string>
    {
  }
}

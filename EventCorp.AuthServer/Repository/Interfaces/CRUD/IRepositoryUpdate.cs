using System.Threading.Tasks;

namespace EventCorp.AuthServer.Repository.Interfaces.CRUD
{
    internal interface IRepositoryUpdate<T, TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<bool> UpdateAsync(T model);
    }
}

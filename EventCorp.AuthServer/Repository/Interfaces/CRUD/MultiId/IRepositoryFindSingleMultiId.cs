using System.Threading.Tasks;

namespace EventCorp.AuthServer.Repository.Interfaces.CRUD.MultiId
{
    internal interface IRepositoryFindSingleMultiId<T, in TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<T> FindAsync(TIdProperty[] id);
    }
}

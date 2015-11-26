using System.Threading.Tasks;

namespace EventCorp.AuthServer.Repository.Interfaces.CRUD.SingleID
{
    interface IRepositoryFindSingle<T, in TIdProperty> where T : class, IEntity<TIdProperty>
    {
        Task<T> FindAsync(TIdProperty id);
    }
}

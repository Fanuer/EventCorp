using System.Threading.Tasks;

namespace EventCorps.Helper.DBAccess.Interfaces.SingleId
{
    public interface IRepositoryAddAndDelete<T, in TIdProperty> where T : class, IEntity<TIdProperty>
    {
        Task<bool> AddAsync(T model);
        Task<bool> RemoveAsync(TIdProperty id);
        Task<bool> RemoveAsync(T model);
        Task<bool> ExistsAsync(TIdProperty id);
    }
}
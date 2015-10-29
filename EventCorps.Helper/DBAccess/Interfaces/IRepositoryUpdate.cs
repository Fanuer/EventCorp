using System.Threading.Tasks;

namespace EventCorps.Helper.DBAccess.Interfaces
{
    public interface IRepositoryUpdate<T, TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<bool> UpdateAsync(T model);
    }
}
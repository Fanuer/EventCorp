using System.Threading.Tasks;

namespace EventCorps.Helper.DBAccess.Interfaces.MultiId
{
    public interface IRepositoryFindSingleMultiId<T, in TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<T> FindAsync(TIdProperty[] id);
    }
}
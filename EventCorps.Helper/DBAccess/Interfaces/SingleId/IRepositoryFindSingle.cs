using System.Threading.Tasks;

namespace EventCorps.Helper.DBAccess.Interfaces.SingleId
{
    public interface IRepositoryFindSingle<T, in TIdProperty> where T : class, IEntity<TIdProperty>
    {
        Task<T> FindAsync(TIdProperty id);
}
}
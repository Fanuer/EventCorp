using System.Linq;
using System.Threading.Tasks;

namespace EventCorps.Helper.DBAccess.Interfaces
{
    public interface IRepositoryFindAll<T>
    {
        Task<IQueryable<T>> GetAllAsync();
    }
}
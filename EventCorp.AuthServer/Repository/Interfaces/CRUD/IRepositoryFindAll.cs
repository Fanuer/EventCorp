using System.Linq;
using System.Threading.Tasks;

namespace EventCorp.AuthServer.Repository.Interfaces.CRUD
{
    public interface IRepositoryFindAll<T>
    {
        Task<IQueryable<T>> GetAllAsync();
  }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Interfaces.Repositories;
using EventCorps.Helper.DBAccess;

namespace EventCorp.AuthorizationServer.Repository
{
    public class FileRepository : GenericRepository<UserFile, Guid>, IFileRepository
    {
        public FileRepository(DbContext ctx) : base(ctx)
        {
        }

        public override async Task<IQueryable<UserFile>> GetAllAsync()
        {
            var result = new List<UserFile>();
            var context = this.DBContext as AuthContext;

            if (context != null)
            {
                result = await context.Files.ToListAsync();
            }
            return result.AsQueryable();
        }

        public override async Task<UserFile> FindAsync(Guid id)
        {
            UserFile result = null;
            var context = this.DBContext as AuthContext;
            if (context != null)
            {
                result = await context.Files.FindAsync(id);
            }
            return result;
        }
    }
}

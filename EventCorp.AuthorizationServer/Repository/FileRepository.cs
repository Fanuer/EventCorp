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

    }
}

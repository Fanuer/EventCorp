using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.AuthorizationServer.Entites;
using EventCorps.Helper.DBAccess.Interfaces;
using EventCorps.Helper.DBAccess.Interfaces.SingleId;

namespace EventCorp.AuthorizationServer.Interfaces.Repositories
{
    public interface IFileRepository: IRepositoryAddAndDelete<UserFile, Guid>, IRepositoryFindAll<UserFile>, IRepositoryFindSingle<UserFile, Guid>, IRepositoryUpdate<UserFile, Guid>
    {

    }
}

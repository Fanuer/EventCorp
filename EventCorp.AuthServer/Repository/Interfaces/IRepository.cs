using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorp.AuthServer.Repository.Interfaces
{
    internal interface IRepository : IDisposable
    {
        IClientRepository Clients { get; }
        IRefreshTokenRepository RefreshTokens { get; }
    }
}

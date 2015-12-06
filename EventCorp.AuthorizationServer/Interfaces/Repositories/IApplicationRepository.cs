using System;

namespace EventCorp.AuthorizationServer.Interfaces.Repositories
{
    public interface IApplicationRepository : IDisposable
    {
        IAudienceRepository Audiences{ get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IFileRepository Files { get; }
    }
}
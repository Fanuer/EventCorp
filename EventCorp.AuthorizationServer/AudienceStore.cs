using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Interfaces.Repositories;
using EventCorp.AuthorizationServer.Repository;
using EventCorps.Helper;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace EventCorp.AuthorizationServer
{
    public class AudiencesStore
    {
        #region Field
        private static AudiencesStore _singleton;
        private readonly ConcurrentDictionary<string, Audience> _audiencesList;
        private readonly IAudienceRepository _rep;
        private bool initialized = false;
        #endregion

        #region Ctor

        public AudiencesStore()
        {
            _rep = new AudienceRepository(AuthContext.Create());
            try
            {
                _audiencesList = new ConcurrentDictionary<string, Audience>();
            }
            catch (Exception e )
            {
                throw e;
            }
            
        }
        #endregion

        #region Methods
        public async Task<Audience> AddAudience(string name)
        {
            if (!initialized)
            {
                await this.Initialize();
            }
            //create clientId
            var clientId = Guid.NewGuid().ToString("N");
            var secret = Utilities.GetHash(name);

            var newAudience = new Audience { Id = clientId, Secret = secret, Name = name };
            if (_audiencesList.TryAdd(clientId, newAudience))
            {
                var peng = await _rep.AddAsync(newAudience);
            }

            return newAudience;
        }

        public Audience FindAudience(string clientId)
        {
            Audience audience = null;
            _audiencesList.TryGetValue(clientId, out audience);
            return audience;
        }

        public async Task RemoveAudience(string clientId)
        {
            if (!initialized)
            {
                await this.Initialize();
            }
            Audience audience = null;
            _audiencesList.TryRemove(clientId, out audience);
            await _rep.RemoveAsync(clientId);
        }

        private async Task Initialize()
        {
            var clients = await _rep.GetAllAsync();
            foreach (var client in clients)
            {
                _audiencesList.TryAdd(client.Id, client);
            }
            initialized = true;
        }
        #endregion

        #region Properties
        public static AudiencesStore Instance => _singleton ?? (_singleton = new AudiencesStore());
        #endregion

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EventCorps.Helper.Models;

namespace EventCorps.Helper.HttpAccess
{
    public class UserManagement:ManagementBase
    {
        #region Field

        #endregion

        #region Ctor

        public UserManagement(HttpClient client):base(client)
        {
        }
        #endregion

        #region Methods

        public async Task<UserStatisticsModel> GetUserStatisticsAsync(string bearer)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", bearer);
            return await GetAsync<UserStatisticsModel>("/api/accounts/users/statistics");
        }

        public async Task<UserModel> GetCurrentUserAsync(string bearer)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", bearer);
            return await GetAsync<UserModel>("/api/accounts/currentUser");
        }

        public async Task<IEnumerable<string>> GetAdminEmailAddressesAsync(string bearer)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", bearer);
            return await GetAsync<IEnumerable<string>>("/api/accounts/admins/emails");
        }

        #endregion
    }
}

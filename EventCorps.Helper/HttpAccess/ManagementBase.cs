

using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace EventCorps.Helper.HttpAccess
{
    public abstract class ManagementBase
    {
        #region Field
        #endregion

        #region ctor

        internal ManagementBase(HttpClient client)
        {
            this.Client = client;
        }
        #endregion
        #region Methods
        protected async Task<T> GetAsync<T>(string url, params object[] args)
        {
            var response = await Client.GetAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new ServerException(response);
        }

        #endregion

        #region Properties

        protected HttpClient Client { get; set; }
        #endregion
    }
}

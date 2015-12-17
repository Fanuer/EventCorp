using System.Net.Http;
using System.Threading.Tasks;
using EventCorps.Helper.Models;

namespace EventCorps.Helper.HttpAccess
{
    public class EventManagement
    {
        #region Field

        private HttpClient _client;

        #endregion

        #region Ctor
        public EventManagement(HttpClient client)
        {
            _client = client;
        }
        #endregion

        #region Method

        public async Task<EventStatisticsModel> GetStatistics(string bearer)
        {
            
        }

        public async Task<ListModel<EventModel>> GetRecommendations(string bearer)
        {
            
        }
        #endregion
    }
}
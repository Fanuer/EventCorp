using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EventCorps.Helper.Models;

namespace EventCorps.Helper.HttpAccess
{
  public class EventManagement : ManagementBase
  {
    #region Field

    private HttpClient _client;

    #endregion

    #region Ctor
    public EventManagement(HttpClient client)
      :base(client)
    {
      _client = client;
    }
    #endregion

    #region Method

    public async Task<EventStatisticsModel> GetStatisticsAsync(string bearer)
    {
      Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", bearer);
      return await GetAsync<EventStatisticsModel>("/api/events/statistics");
    }

    public async Task<ListModel<EventModel>> GetRecommendationsAsync(string bearer)
    {
      Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", bearer);
      return await GetAsync<ListModel<EventModel>>("/api/events/recommendations");
    }
    #endregion
  }
}
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using EventCorp.Clients;
using EventCorps.Helper.Models;
using Microsoft.AspNet.Identity.Owin;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.Recommendation.Controller
{
  [Authorize]
  [RoutePrefix("api/statistics")]
  [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occurred")]
  [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
  public class StatisticsController : ApiController
  {
    private SimpleManagementSession _session = null;


    /// <summary>
    /// Gets the users recommendations
    /// </summary>
    /// <returns></returns>
    [Route("")]
    [HttpGet]
    [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatisticsModel))]
    public async Task<IHttpActionResult> GetStatistics()
    {
      var eventStatistics = await this.AppManagementSession.Events.GetStatisticsAsync(this.BearerToken);
      var userStatistics = await this.AppManagementSession.Users.GetUserStatisticsAsync(this.BearerToken);
      return Ok(new StatisticsModel()
      {
        Events = eventStatistics,
        Users = userStatistics
      });
    }

    /// <summary>
    /// Repository
    /// </summary>
    internal SimpleManagementSession AppManagementSession
    {
      get { return this._session ?? Request.GetOwinContext().Get<SimpleManagementSession>(); }
    }


    protected string BearerToken
    {
      get
      {
        return this.Request.Headers.Authorization.Parameter;
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Models;
using EventCorps.Helper;

namespace EventCorp.AuthorizationServer.Controllers
{
  [RoutePrefix("api/audience")]
  [Authorize(Roles = "Admin")]
  public class AudienceController : BaseApiController
  {
    /// <summary>
    /// Registers the calling client to the AuthServer
    /// </summary>
    /// <param name="audienceModel">Client name</param>
    /// <returns></returns>
    [Route("")]
    [HttpPost]
    public async Task<IHttpActionResult> AddAudience(AudienceModel audienceModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var clientId = Guid.NewGuid().ToString("N");
      var name = audienceModel.Name;
      var secret = Utilities.GetHash(name);

      var newAudience = new Audience(clientId, secret, name);
      await this.AppRepository.Audiences.AddAsync(newAudience);
      return Ok();
    }

    /// <summary>
    /// Removes the calling audience from the list of registered clients
    /// </summary>
    /// <returns></returns>
    [Route("")]
    [HttpDelete]
    public async Task<IHttpActionResult> RemoveAudience()
    {
      var identity = User.Identity as ClaimsIdentity;

      var clientID = identity.Claims.FirstOrDefault(x => x.Type.Equals("clientId"));
      if (clientID == null || String.IsNullOrWhiteSpace(clientID.Value))
      {
        return this.BadRequest("No ClientID found");
      }
      var removeResult = await AppRepository.Audiences.RemoveAsync(clientID.Value);
      if (removeResult)
      {
        return Ok();
      }
      return BadRequest("No Client found");
    }
  }
}

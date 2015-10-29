using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Models;

namespace EventCorp.AuthorizationServer.Controllers
{
    [RoutePrefix("api/audience")]
    public class AudienceController : ApiController
    {
        /// <summary>
        /// Registers the calling client to the AuthServer
        /// </summary>
        /// <param name="audienceModel">Client name</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddAudience([FromBody]AudienceModel audienceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newAudience = await AudiencesStore.Instance.AddAudience(audienceModel.Name);
            return Ok(newAudience);
        }

        /// <summary>
        /// Removes the calling audience from the list of registered clients
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IHttpActionResult> RemoveAudience()
        {
            var identity = User.Identity as ClaimsIdentity;

            var clientID = identity.Claims.FirstOrDefault(x => x.Type.Equals("clientId"));
            if (clientID == null || String.IsNullOrWhiteSpace(clientID.Value))
            {
                return this.BadRequest("No ClientID found");
            }
            await AudiencesStore.Instance.RemoveAudience(clientID.Value);
            return Ok();
        }
    }
}

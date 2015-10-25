using System;
using System.Collections.Generic;
using System.Linq;
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
        [Route("")]
        public IHttpActionResult Post(AudienceModel audienceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newAudience = AudiencesStore.AddAudience(audienceModel.Name);
            return Ok(newAudience);
        }
    }
}

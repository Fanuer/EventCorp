using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.EventServer.Controller
{
    /// <summary>
    /// Handles User-based Actions
    /// </summary>
    [Authorize]
    [RoutePrefix("api/events")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occured")]
    public class EventsController : ApiController
    {
        public async Task<IHttpActionResult> GetOpenTasks()
        {
            return Ok();
        }
    }
}

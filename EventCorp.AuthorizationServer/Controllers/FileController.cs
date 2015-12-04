using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.AuthorizationServer.Controllers
{
  /// <summary>
  /// Controler to manage User roles
  /// </summary>
  [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
  [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occured")]
  //[Authorize(Roles = "Admin")]
  [RoutePrefix("api/file")]
  public class FileController : BaseApiController
  {
    [Route("fileRequest")]
    [HttpPost]
    public IHttpActionResult UploadAsFileRequest()
    {
      return Ok();
    }

    [Route("fileBase")]
    [HttpPost]
    public IHttpActionResult UploadAsFileBase(HttpPostedFileBase file)
    {
      return Ok();
    }

    [Route("multipart")]
    [HttpPost]
    public IHttpActionResult UploadAsMultiPart()
    {
      return Ok();
    }
  }
}

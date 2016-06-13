using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EventCorps.Helper;
using EventCorps.Helper.Enums;
using EventCorps.Helper.Models;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.AuthorizationServer.Controllers
{
  [RoutePrefix("api/enums")]
  [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occurred")]
  public class EnumController : BaseApiController
    {
    /// <summary>
    /// Returns all Genders
    /// </summary>
    [ResponseType(typeof(IEnumerable<EnumValueModel<GenderType>>))]
    [Route("gender")]
    [HttpGet]
    public IHttpActionResult GetGenderValues()
    {
      return Ok(GetEnumValues<GenderType>());
    }

    /// <summary>
    /// Returns all Genders
    /// </summary>
    [ResponseType(typeof(IEnumerable<EnumValueModel<EventType>>))]
    [Route("events")]
    [HttpGet]
    public IHttpActionResult GetEventValues()
    {
      return Ok(GetEnumValues<EventType>());
    }


    private IEnumerable<EnumValueModel<T>> GetEnumValues<T>()
    {
      var enumType = typeof(T);
      if (!enumType.IsEnum)
      {
        throw new ArgumentException("Only enum types are allowed");
      }
      var values = Enum.GetValues(enumType).Cast<T>();
      return values.Select(x => new EnumValueModel<T>()
      {
        Value = x,
        DisplayName = x.GetDisplayName()
      });
    }


  }
}

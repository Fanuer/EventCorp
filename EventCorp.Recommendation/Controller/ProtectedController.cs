﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace EventCorp.Recommendation.Controller
{
  [Authorize]
  [RoutePrefix("api/protected")]
  public class ProtectedController : ApiController
  {
    [Route("")]
    public IEnumerable<object> Get()
    {
      var identity = User.Identity as ClaimsIdentity;

      return identity.Claims.Select(c => new
      {
        Type = c.Type,
        Value = c.Value
      });
    }
  }
}

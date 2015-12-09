using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCorp.EventServer.Interfaces
{
  internal interface IEventServerRepository: IDisposable
  {
    IEventRepository Events { get; set; }
    ISubscriberRepository Subscribers { get; set; }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventCorp.EventServer.Interfaces
{
  public interface IEventServerRepository
  {
    public IEventRepository Events { get; set; }
    public ISubscriberRepository Subscribers { get; set; }
  }
}
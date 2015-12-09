using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventCorp.EventServer.Entities;
using EventCorp.EventServer.Interfaces;
using EventCorps.Helper.DBAccess;

namespace EventCorp.EventServer.Repositories
{
  public class EventServerRepository:IEventServerRepository
  {
    public IEventRepository Events { get; set; }
    public ISubscriberRepository Subscribers { get; set; }
  }

  
}
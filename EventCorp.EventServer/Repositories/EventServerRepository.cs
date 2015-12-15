using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventCorp.EventServer.Entities;
using EventCorp.EventServer.Interfaces;
using EventCorps.Helper.DBAccess;

namespace EventCorp.EventServer.Repositories
{
  internal class EventServerRepository : IEventServerRepository
  {
    private readonly EventContext _ctx;


    public EventServerRepository()
    {
      _ctx = new EventContext();
      Events = new EventRepository(_ctx);
      Subscribers = new SubscriberRepository(_ctx);
    }

    public IEventRepository Events { get; set; }
    public ISubscriberRepository Subscribers { get; set; }

    public void Dispose()
    {
      _ctx.Dispose();

    }
  }


}
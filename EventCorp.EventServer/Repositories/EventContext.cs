using System.Data.Entity;
using EventCorp.EventServer.Entities;

namespace EventCorp.EventServer.Repositories
{
  internal class EventContext: DbContext
  {
    #region Ctor

    public EventContext()
      : base("DefaultConnection")
    {
      
    }
    #endregion

    #region Properties

    public IDbSet<Event> Events { get; set; }
    public IDbSet<Subscriber> Subscribers { get; set; }
    #endregion
  }
}

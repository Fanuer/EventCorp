using System.Data.Entity;
using EventCorp.EventServer.Entities;
using EventCorp.EventServer.Interfaces;

namespace EventCorp.EventServer.Repositories
{
    internal class EventContext : DbContext
    {
        #region Ctor

        public EventContext()
          : base("DefaultConnection")
        {

        }
        #endregion

        #region Methods
        public static EventContext Create()
        {
            return new EventContext();
        }

        internal static IEventServerRepository CreateRepository()
        {
            return new EventServerRepository();
        }

        #endregion

        #region Properties

        public DbSet<Event> Events { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        #endregion
    }
}

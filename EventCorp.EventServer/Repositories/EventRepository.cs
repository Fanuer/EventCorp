using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EventCorp.EventServer.Entities;
using EventCorp.EventServer.Interfaces;
using EventCorps.Helper.DBAccess;

namespace EventCorp.EventServer.Repositories
{
    internal class EventRepository : GenericRepository<Event, int>, IEventRepository
    {
        public EventRepository(EventContext ctx) : base(ctx)
        {

        }

        public override async Task<Event> FindAsync(int id)
        {
            Event result = null;
            var context = this.DBContext as EventContext;
            if (context != null)
            {
                result = await context.Events.FindAsync(id);
            }
            return result;
        }

        public override async Task<IQueryable<Event>> GetAllAsync()
        {
            var result = new List<Event>();
            var context = this.DBContext as EventContext;

            if (context != null)
            {
                result = await context.Events.ToListAsync();
                return result.AsQueryable();
            }
            return result.AsQueryable();
        }
    }
}
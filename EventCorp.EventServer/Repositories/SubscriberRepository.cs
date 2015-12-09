using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.EventServer.Entities;
using EventCorp.EventServer.Interfaces;
using EventCorps.Helper.DBAccess;

namespace EventCorp.EventServer.Repositories
{
    public class SubscriberRepository: GenericRepository<Subscriber, int>, ISubscriberRepository
    {
        public SubscriberRepository(DbContext ctx) : base(ctx)
        {
        }

        public override async Task<IQueryable<Subscriber>> GetAllAsync()
        {
            var result = new List<Subscriber>();
            var context = this.DBContext as EventContext;

            if (context != null)
            {
                result = await context.Subscribers.ToListAsync();
            }
            return result.AsQueryable();
        }

        public override async Task<Subscriber> FindAsync(int id)
        {
            Subscriber result = null;
            var context = this.DBContext as EventContext;
            if (context != null)
            {
                result = await context.Subscribers.FindAsync(id);
            }
            return result;
        }
    }
}

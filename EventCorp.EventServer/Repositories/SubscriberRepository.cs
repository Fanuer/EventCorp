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
    }
}

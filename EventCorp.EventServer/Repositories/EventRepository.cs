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

    public async Task<IList<Event>> GetEventsAsync(string userId, string searchTerm = "", bool onlyOpenEvents = true, bool onlySubscriped = false, DateTime? tilDate = null, int pageSize = 25, int page = 0)
    {
      return await GetOrderedEvents(userId, searchTerm, onlyOpenEvents, onlySubscriped, tilDate)
                    .Skip(pageSize > 0 && page > 0 ? page * pageSize : 0)
                    .Take(pageSize > 0 ? pageSize : 25)
                    .Include(x => x.Subscribers)
                    .ToListAsync();
    }

    public async Task<int> GetCountAsync(string userId, string searchTerm, bool onlyOpenEvents, bool onlySubscriped, DateTime? tilDate)
    {
      return await GetOrderedEvents(userId, searchTerm, onlyOpenEvents, onlySubscriped, tilDate).CountAsync();
    }

    private IOrderedQueryable<Event> GetOrderedEvents(string userId, string searchTerm = "", bool onlyOpenEvents = true, bool onlySubscriped = false, DateTime? tilDate = null)
    {
      var dbSet = this.GetTypedDBSet<Event>();
      var searchTermNotSet = String.IsNullOrWhiteSpace(searchTerm);
      if (dbSet == null)
      {
        throw new NullReferenceException("No DBSet for Events found");
      }
      if (!searchTermNotSet)
      {
        searchTerm = searchTerm.ToLower();
      }
      return dbSet
        .Where(x => !onlyOpenEvents || (x.StartTime >= DateTime.UtcNow))
        .Where(x => searchTermNotSet || x.Name.ToLower().Contains(searchTerm) || x.Place.ToLower().Contains(searchTerm))
        .Where(x => !onlySubscriped || x.Subscribers.Any(sub => sub.UserId.ToString().Equals(userId)))
        .Where(x=> !tilDate.HasValue || (x.StartTime <= tilDate.Value))
        .OrderBy(x => x.StartTime)
        .ThenBy(x => x.Name);
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.EventServer.Entities;
using EventCorps.Helper.DBAccess.Interfaces;
using EventCorps.Helper.DBAccess.Interfaces.SingleId;
using EventCorps.Helper.Models;

namespace EventCorp.EventServer.Interfaces
{
  internal interface IEventRepository: IRepositoryAddAndDelete<Event, int>, IRepositoryFindSingle<Event, int>, IRepositoryUpdate<Event, int>, ICountAsync<Event>
  {
    Task<IList<Event>> GetEventsAsync(string userId, string searchTerm, bool onlyOpenEvents, bool onlySubscriped, DateTime? tilDate, int pageSize, int page);
    Task<int> GetCountAsync(string userId, string searchTerm, bool onlyOpenEvents, bool onlySubscriped, DateTime? tilDate);
  }
}

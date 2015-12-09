using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.EventServer.Entities;
using EventCorps.Helper.DBAccess.Interfaces;
using EventCorps.Helper.DBAccess.Interfaces.SingleId;

namespace EventCorp.EventServer.Interfaces
{
  internal interface ISubscriberRepository:IRepositoryAddAndDelete<Subscriber, int>, IRepositoryFindSingle<Subscriber, int>, IRepositoryFindAll<Subscriber>, IRepositoryUpdate<Subscriber, int>
  {
  }
}

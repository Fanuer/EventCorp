using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorps.Helper.DBAccess.Interfaces
{
  public interface ICountAsync<T> where T:class
  {
    Task<int> CountAsync<T>() where T : class;
  }
}

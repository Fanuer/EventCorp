using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorps.Helper.Models
{
  public class ListModel<T> where T:class 
  {
    public ListModel(IEnumerable<T> entries = null, int pageSize = 25, int page = 0, int allEntriesCount = 25)
    {
      Entries = entries ?? new List<T>();
      PageSize = pageSize;
      Page = page;
      AllEntriesCount = allEntriesCount;
    }

    public int PageSize{ get; set; }
    public int Page { get; set; }
    public int AllEntriesCount { get; set; }
    public IEnumerable<T> Entries { get; set; }
  }
}

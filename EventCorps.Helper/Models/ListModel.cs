using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorps.Helper.Models
{
  public class ListModel<T> where T:class 
  {
    public int PageSize{ get; set; }
    public int Page { get; set; }
    public int AllEntriesCount { get; set; }
    public IList<T> Entries { get; set; }
  }
}

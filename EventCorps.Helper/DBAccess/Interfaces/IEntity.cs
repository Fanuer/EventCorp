using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorps.Helper.DBAccess.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}

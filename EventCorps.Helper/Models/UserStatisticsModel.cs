using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorps.Helper.Models
{
    public class UserStatisticsModel
    {
        public long RegisteredUsers { get; set; }
        public long RegisteredAdmins { get; set; }
        public int LockedOutUsers { get; set; }
    }
}

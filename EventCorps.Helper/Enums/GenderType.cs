using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorps.Helper.Enums
{
    public enum GenderType
    {
        [Display(ResourceType = typeof(Resources), Name = "Enum_Gender_Male")]
        Male,
        [Display(ResourceType = typeof(Resources), Name = "Enum_Gender_Female")]
        Female,
        [Display(ResourceType = typeof(Resources), Name = "Enum_Other")]
        Other
    }
}

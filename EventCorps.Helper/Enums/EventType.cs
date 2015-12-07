using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorps.Helper.Enums
{
    public enum EventType
    {
        [Display(ResourceType = typeof (Resources), Name = "Enum_EventType_Cultural")]
        Cultural,
        [Display(ResourceType = typeof (Resources), Name = "Enum_EventType_Sports")]
        Sports,
        [Display(ResourceType = typeof (Resources), Name = "Enum_EventType_Education")]
        Education,
        [Display(ResourceType = typeof(Resources), Name = "Enum_Other")]
        Other
    }
}

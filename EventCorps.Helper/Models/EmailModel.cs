using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorps.Helper.Enums;

namespace EventCorps.Helper.Models
{
    public class EmailModel
    {
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public EmailTypes EmailType { get; set; }
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public string Id { get; set; }
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        [EmailAddress]
        public SendToTypes SendTo { get; set; }
    }
}

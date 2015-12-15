using System.ComponentModel.DataAnnotations;

namespace EventCorps.Helper.Models
{
    public class UpdateEventModel:CreateEventModel
    {
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public int Id { get; set; }
    }
}
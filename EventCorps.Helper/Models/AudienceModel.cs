using System.ComponentModel.DataAnnotations;

namespace EventCorps.Helper.Models
{
    public class AudienceModel
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}

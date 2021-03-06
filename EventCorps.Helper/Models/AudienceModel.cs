﻿using System.ComponentModel.DataAnnotations;

namespace EventCorps.Helper.Models
{
    public class AudienceModel
    {
        [MaxLength(100)]
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        public string Name { get; set; }
    }
}

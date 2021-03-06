﻿using System.ComponentModel.DataAnnotations;

namespace EventCorps.Helper.Models
{
    /// <summary>
    /// Login Data
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        [Display(ResourceType = typeof(Resources), Name = "Label_Username")]
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = "Label_Password")]
        public string Password { get; set; }
    }
}

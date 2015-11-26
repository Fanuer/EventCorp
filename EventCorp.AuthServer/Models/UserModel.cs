using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorp.AuthServer.Models
{
    /// <summary>
    /// User data
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Url to receive this object
        /// </summary>
        [Display(ResourceType = typeof (Resources), Name = "Label_Url")]
        public string Url { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof (Resources))]
        [Display(ResourceType = typeof (Resources), Name = "Label_Id")]
        public string Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof (Resources))]
        [Display(ResourceType = typeof (Resources), Name = "Label_Username")]
        public string UserName { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof (Resources))]
        [Display(ResourceType = typeof (Resources), Name = "Label_Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Geburtstag
        /// </summary>
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof (Resources))]
        [Display(ResourceType = typeof (Resources), Name = "Label_DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Vorname
        /// </summary>
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof (Resources))]
        [Display(ResourceType = typeof (Resources), Name = "Label_Forename")]
        public string Forename { get; set; }

        /// <summary>
        /// Nachname
        /// </summary>
        [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof (Resources))]
        [Display(ResourceType = typeof (Resources), Name = "Label_Surname")]
        public string Surname { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorps.Helper.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EventCorps.Helper.Models
{
  public class BaseUserModel
  {
    /// <summary>
    /// User name
    /// </summary>
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    [Display(ResourceType = typeof(Resources), Name = "Label_Username")]
    public string Username { get; set; }

    /// <summary>
    /// User's email
    /// </summary>
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    [Display(ResourceType = typeof(Resources), Name = "Label_Email")]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Alter
    /// </summary>
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    [Display(ResourceType = typeof(Resources), Name = "Label_DateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Confirm Password. Must be equal to the given Password
    /// </summary>
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Resources), Name = "Label_Forename")]
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public string Forename { get; set; }

    /// <summary>
    /// Confirm Password. Must be equal to the given Password
    /// </summary>
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Resources), Name = "Label_Surname")]
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public string Surname { get; set; }
    [Required]
    public string City { get; set; }

    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    [JsonConverter(typeof(StringEnumConverter))]
    [Display(ResourceType = typeof(Resources), Name = "Label_Gender")]
    public GenderType GenderType { get; set; }
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    [JsonConverter(typeof(StringEnumConverter))]
    [Display(ResourceType = typeof(Resources), Name = "Label_FavoriteEvent")]
    public EventType FavoriteEventType { get; set; }
  }
}

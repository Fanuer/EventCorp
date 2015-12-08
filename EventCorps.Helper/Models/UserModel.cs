using System;
using System.ComponentModel.DataAnnotations;
using EventCorps.Helper.Enums;

namespace EventCorps.Helper.Models
{
  /// <summary>
  /// User data
  /// </summary>
  public class UserModel:BaseUserModel
  {
    /// <summary>
    /// Url to receive this object
    /// </summary>
    [Display(ResourceType = typeof(Resources), Name = "Label_Url")]
    public string Url { get; set; }
    /// <summary>
    /// User Id
    /// </summary>
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    [Display(ResourceType = typeof(Resources), Name = "Label_Id")]
    public string Id { get; set; }

    /// <summary>
    /// Id of the users avatar file
    /// </summary>
    public Guid? AvatarId { get; set; }

  }
}

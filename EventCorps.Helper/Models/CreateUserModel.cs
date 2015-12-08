using System;
using System.ComponentModel.DataAnnotations;

namespace EventCorps.Helper.Models
{
  /// <summary>
  /// Data to register a User
  /// </summary>
  public class CreateUserModel : BaseUserModel
  {
    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    [StringLength(100, ErrorMessageResourceName = "Error_StringLength", ErrorMessageResourceType = typeof(Resources), MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Resources), Name = "Label_Password")]
    public string Password { get; set; }

    /// <summary>
    /// Confirm Password. Must be equal to the given Password
    /// </summary>
    [DataType(DataType.Password)]
    [Display(ResourceType = typeof(Resources), Name = "Label_ConfirmPassword")]
    [Compare("Password", ErrorMessageResourceName = "Error_PasswordsNotEqual", ErrorMessageResourceType = typeof(Resources))]
    public string ConfirmPassword { get; set; }


  }
}

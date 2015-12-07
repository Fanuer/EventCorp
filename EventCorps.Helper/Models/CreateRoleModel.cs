using System.ComponentModel.DataAnnotations;

namespace EventCorps.Helper.Models
{
  public class CreateRoleModel
  {
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    [StringLength(256, ErrorMessageResourceName = "Error_StringLength", ErrorMessageResourceType = typeof(Resources), MinimumLength = 2)]
    [Display(ResourceType = typeof(Resources), Name = "Label_RoleName")]
    public string Name { get; set; }
  }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace EventCorps.Helper.Models
{
  public class UploadFileViewModel : EntryModel<Guid>
  {
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public long ContentSize { get; set; }
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public string ContentType { get; set; }
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public bool Global { get; set; }
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public Guid Owner { get; set; }
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public bool IsPermanent { get; set; }
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public DateTime Created { get; set; }
  }
}

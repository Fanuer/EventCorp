﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventCorps.Helper.Models
{
  /// <summary>
  /// Definies a Role and the added and removed users
  /// </summary>
  public class UsersInRoleModel
  {
    /// <summary>
    /// Role Id
    /// </summary>
    [Display(ResourceType = typeof(Resources), Name = "Label_Id")]
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public string Id { get; set; }
    /// <summary>
    /// Enrolled User
    /// </summary>
    [Display(ResourceType = typeof(Resources), Name = "Label_UsersEnrolledToRole")]
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public List<string> EnrolledUsers { get; set; }
    /// <summary>
    /// Removed User
    /// </summary>
    [Display(ResourceType = typeof(Resources), Name = "Label_UsersRemovedFromRole")]
    [Required(ErrorMessageResourceName = "Error_Required", ErrorMessageResourceType = typeof(Resources))]
    public List<string> RemovedUsers { get; set; }
  }
}
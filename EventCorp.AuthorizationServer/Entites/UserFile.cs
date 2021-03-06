﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorps.Helper.DBAccess.Interfaces;

namespace EventCorp.AuthorizationServer.Entites
{
  public class UserFile : IEntity<Guid>
  {
    private Guid _userId;

    #region Ctor
    public UserFile(string name = "", string contentType = "application/octet-stream", byte[] content = null, bool isTemp = true, DateTime? created = null, bool global = false)
    {
      ContentType = contentType;
      Content = content ?? new byte[0];
      Name = name;
      IsTemp = isTemp;
      CreatedUTC = created ?? DateTime.UtcNow;
      Global = global;
    }

    public UserFile() : this("")
    {

    }
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string ContentType { get; set; }
    [Required]
    public byte[] Content { get; set; }
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// If this is set to true, the server will delete this entry within 24 hours
    /// </summary>
    public bool IsTemp { get; set; }
    /// <summary>
    /// Creation Date as UTC
    /// </summary>
    public DateTime CreatedUTC { get; set; }

    public Guid Owner => _userId;

    /// <summary>
    /// File Owner
    /// </summary>
    public string UserId
    {
      get { return this._userId.ToString(); }
      set
      {
        var temp = Guid.Empty;
        if (Guid.TryParse(value, out temp))
        {
          this._userId = temp;
        }
      }
    }

    public virtual User User { get; set; }

    /// <summary>
    /// Shall this file be used by other Users
    /// </summary>
    public bool Global { get; set; }
    #endregion
  }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventCorps.Helper.DBAccess.Interfaces;
using EventCorps.Helper.Enums;

namespace EventCorp.EventServer.Entities
{
  public class Subscriber:IEntity<int>
  {
    [Key]
    public int Id { get; set; }
    [Index("IX_UserToEvent", 1, IsUnique = true)]
    public Guid UserId { get; set; }
    [Required]
    public DateTime SubscriptionTime { get; set; }
    [Required]
    public SubscriptionType Status { get; set; }
    [Index("IX_UserToEvent", 2, IsUnique = true)]
    public int EventId { get; set; }
    public virtual Event Event { get; set; }
    

  }
}
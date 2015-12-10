using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventCorps.Helper.DBAccess.Interfaces;
using EventCorps.Helper.Enums;

namespace EventCorp.EventServer.Entities
{
    public class Event : IEntity<int>
    {
        public Event()
        {
            Subscribers = new List<Subscriber>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public EventType Type { get; set; }
        [Required]
        public int MaxNumberOfParticipants { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public string Place { get; set; }

        public virtual ICollection<Subscriber> Subscribers { get; set; }
    }
}
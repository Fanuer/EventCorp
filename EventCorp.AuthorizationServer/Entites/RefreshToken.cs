using System;
using System.ComponentModel.DataAnnotations;
using EventCorps.Helper.DBAccess.Interfaces;

namespace EventCorp.AuthorizationServer.Entites
{
    public class RefreshToken : IEntity<string>
    {
        public RefreshToken()
        {
                
        }

        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(50)]
        public string ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        [Required]
        public string ProtectedTicket { get; set; }
    }
}
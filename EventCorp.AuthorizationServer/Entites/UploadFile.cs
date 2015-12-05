using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorps.Helper.DBAccess.Interfaces;

namespace EventCorp.AuthorizationServer.Entites
{   
    public class UploadFile : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public byte[] Data { get; set; }
        public long Size { get; set; }
        [Required]
        public string MimeType { get; set; }
    }
}

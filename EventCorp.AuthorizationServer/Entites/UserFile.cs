using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorps.Helper.DBAccess.Interfaces;

namespace EventCorp.AuthorizationServer.Entites
{
    public class UserFile : IEntity<Guid>
    {
        public UserFile(string name= "", string contentType = "application/octet-stream", byte[] content = null, bool isTemp = true)
        {
            ContentType = contentType;
            Content = content ?? new byte[0];
            Name = name;
            IsTemp = isTemp;
        }

        public UserFile():this("")
        {
            
        }

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

        public DateTime CreatedUTC { get; set; }
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;
using EventCorps.Helper.DBAccess.Interfaces;

namespace EventCorp.AuthorizationServer.Entites
{
    public class Audience: IEntity<string>
    {
        #region Ctor

        public Audience(string id="", string secret ="", string name="", string allowedOrigin = "*")
        {
        }

        public Audience():this("")
        {
            
        }
        #endregion

        #region Properties
        [Key]
        [MaxLength(32)]
        public string Id { get; set; }

        [MaxLength(80)]
        [Required]
        public string Secret { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public bool Active { get; set; }

        [MaxLength(100)]
        public string AllowedOrigin { get; set; }
        #endregion
    }
}

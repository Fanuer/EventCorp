using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using EventCorps.Helper.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventCorp.AuthorizationServer.Entites
{
    public class User : IdentityUser
    {
        #region ctor

        public User(string username = "", string email = "", string forename = "", string surname = "", DateTime dateOfBirth = default(DateTime))
            :base(username)
        {
            base.Email = email;
            Forename = forename;
            Surname = surname;
            DateOfBirth = dateOfBirth;
        }
        public User()
            : this("")
        {

        }
        #endregion

        #region Method
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType, string clientId ="")
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            if (!String.IsNullOrWhiteSpace(clientId))
            {
                userIdentity.AddClaim(new Claim("clientId", clientId));
                userIdentity.AddClaim(new Claim("userId", userIdentity.GetUserId()));
            }
            
            return userIdentity;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Surname
        /// </summary>
        [Required]
        public string Surname { get; set; }
        /// <summary>
        /// Forname
        /// </summary>
        [Required]
        public string Forename { get; set; }
        /// <summary>
        /// Date of Birth
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// place of residence
        /// </summary>
        [Required]
        public string City { get; set; }

        [Required]
        public GenderType GenderType { get; set; }
        [Required]
        public EventType FavoriteEventType { get; set; }

        public UserFile UserFile { get; set; }
        #endregion
    }
}

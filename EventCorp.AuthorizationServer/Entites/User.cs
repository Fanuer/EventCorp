using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public string Surname { get; set; }
        /// <summary>
        /// Forname
        /// </summary>
        public string Forename { get; set; }
        /// <summary>
        /// Date of Birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        #endregion
    }
}

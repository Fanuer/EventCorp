using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventCorp.AuthServer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        #region ctor

        public ApplicationUser(string username = "", string email = "", string forename = "", string surname = "", DateTime dateOfBirth = default(DateTime))
            :base(username)
        {
            base.Email = email;
            Forename = forename;
            Surname = surname;
            DateOfBirth = dateOfBirth;
        }
        public ApplicationUser()
            : this("")
        {

        }
        #endregion

        #region Method
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType, string clientId = "")
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

        public string Surname { get; set; }
        public string Forename { get; set; }
        public DateTime DateOfBirth { get; set; }
        #endregion

    }
}

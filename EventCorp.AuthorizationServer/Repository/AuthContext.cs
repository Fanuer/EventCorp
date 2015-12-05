using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Interfaces.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventCorp.AuthorizationServer.Repository
{
    /// <summary>
    /// Communcates with the Database
    /// </summary>
    public class AuthContext : IdentityDbContext<User>
    {
        #region Ctor
        public AuthContext()
            : base("DefaultConnection", false)
        {
            this.Database.Log = Console.WriteLine;
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        #endregion

        #region Methods
        public static AuthContext Create()
        {
            return new AuthContext();
        }

        internal static IApplicationRepository CreateRepository()
        {
            return new ApplicationRepository();
        }

        #endregion

        #region Properties
        public DbSet<Audience> Audiences { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UploadFile> Files { get; set; }
        #endregion

    }
}

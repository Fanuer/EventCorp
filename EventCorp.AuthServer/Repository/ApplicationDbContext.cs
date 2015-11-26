using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventCorp.AuthServer.Entities;
using EventCorp.AuthServer.Repository.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventCorp.AuthServer.Repository
{
    /// <summary>
    /// Communcates with the Database
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Ctor
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Database.Log = Console.WriteLine;
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        #endregion

        #region Methods
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        internal static IRepository CreateRepository()
        {
            return new ApplicationRepository();
        }
        #endregion


        #region Properties

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        #endregion

    }
}

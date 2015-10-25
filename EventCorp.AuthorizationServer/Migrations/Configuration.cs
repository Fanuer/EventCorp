using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventCorp.AuthorizationServer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AuthContext context)
        {
            var manager = new UserManager<User>(new UserStore<User>(new AuthContext()));

            var user = new User()
            {
                UserName = "Stefan",
                Email = "Stefan@fIT.com",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1987, 12, 13),
                Forename = "Stefan",
                Surname = "Suermann"
            };
            manager.Create(user, "Test1234");

            user = new User()
            {
                UserName = "Kevin",
                Email = "Kevin@fIT.com",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1993, 7, 4),
                Forename = "Kevin",
                Surname = "Schie"
            };
            manager.Create(user, "Test1234");

            // Added Roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AuthContext()));

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var stefan = manager.FindByName("Stefan");
            var kevin = manager.FindByName("Kevin");
            manager.AddToRoles(stefan.Id, "User", "Admin");
            manager.AddToRoles(kevin.Id, "User", "Admin");
        }
    }
}

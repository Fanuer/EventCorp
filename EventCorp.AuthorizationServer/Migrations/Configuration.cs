using System.Data.Entity.Validation;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Repository;
using EventCorps.Helper.Enums;
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
            try
            {
                var manager = new UserManager<User>(new UserStore<User>(new AuthContext()));

                var user = new User()
                {
                    UserName = "Stefan",
                    Email = "Stefan@fIT.com",
                    EmailConfirmed = true,
                    DateOfBirth = new DateTime(1987, 12, 13),
                    Forename = "Stefan",
                    Surname = "Suermann",
                    City = "Hattingen",
                    GenderType = GenderType.Male,
                    FavoriteEventType = EventType.Cultural
                };
                manager.Create(user, "Test1234");

                user = new User()
                {
                    UserName = "Kevin",
                    Email = "Kevin@fIT.com",
                    EmailConfirmed = true,
                    DateOfBirth = new DateTime(1993, 7, 4),
                    Forename = "Kevin",
                    Surname = "Schie",
                    City = "Dortmund",
                    GenderType = GenderType.Male,
                    FavoriteEventType = EventType.Cultural
                };
                manager.Create(user, "Test1234");

                user = new User()
                {
                    UserName = "Bobi",
                    Email = "bob4o91.lovech@gmail.com",
                    EmailConfirmed = true,
                    DateOfBirth = new DateTime(1993, 7, 4),
                    Forename = "Bobi",
                    Surname = "Gidulski",
                    City = "Dortmund",
                    GenderType = GenderType.Male,
                    FavoriteEventType = EventType.Cultural
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
                var bobi = manager.FindByName("Bobi");
                manager.AddToRoles(bobi.Id, "User", "Admin");
                manager.AddToRoles(kevin.Id, "User", "Admin");
                manager.AddToRoles(stefan.Id, "User", "Admin");

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }        }
    }
}

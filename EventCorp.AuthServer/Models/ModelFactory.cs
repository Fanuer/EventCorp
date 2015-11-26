using System;
using System.Net.Http;
using System.Web.Http.Routing;
using EventCorp.AuthServer.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventCorp.AuthServer.Models
{
    public class ModelFactory
    {
        #region Field
        private readonly UrlHelper _UrlHelper;
        #endregion

        #region Ctor
        public ModelFactory(HttpRequestMessage request)
        {
            _UrlHelper = new UrlHelper(request);
        }
        #endregion

        #region Method
        public UserModel CreateViewModel(ApplicationUser datamodel)
        {
            if (datamodel == null) { throw new ArgumentNullException("datamodel"); }
            return new UserModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = datamodel.Id }),
                Id = datamodel.Id,
                UserName = datamodel.UserName,
                Email = datamodel.Email,
                DateOfBirth = datamodel.DateOfBirth,
                Surname = datamodel.Surname,
                Forename = datamodel.Forename
            };
        }
        /*
        public RoleModel CreateViewModel(IdentityRole datamodel)
        {
            if (datamodel == null)
            {
                throw new ArgumentNullException("datamodel");
            }
            return new RoleModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = datamodel.Id }),
                Id = datamodel.Id,
                Name = datamodel.Name
            };
        }

        public RefreshTokenModel CreateViewModel(RefreshToken datamodel)
        {
            if (datamodel == null)
            {
                throw new ArgumentNullException("datamodel");
            }

            return new RefreshTokenModel()
            {
                Id = datamodel.Id,
                ClientId = datamodel.ClientId,
                Subject = datamodel.Subject,
                ExpiresUtc = datamodel.ExpiresUtc,
                IssuedUtc = datamodel.IssuedUtc
            };
        }
        */
        internal ApplicationUser CreateModel(UserModel model, ApplicationUser datamodel = null)
        {
            var result = datamodel ?? new ApplicationUser();
            result.DateOfBirth = model.DateOfBirth;
            result.UserName = model.UserName;
            result.Email = model.Email;
            result.Forename = model.Forename;
            result.Surname = model.Surname;
            return result;
        }
        #endregion
    }
}

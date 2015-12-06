using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using EventCorp.AuthorizationServer.Entites;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventCorp.AuthorizationServer.Models
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
        public UserModel CreateViewModel(User datamodel)
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

        internal User CreateModel(UserModel model, User datamodel = null)
        {
            var result = datamodel ?? new User();
            result.DateOfBirth = model.DateOfBirth;
            result.UserName = model.UserName;
            result.Email = model.Email;
            result.Forename = model.Forename;
            result.Surname = model.Surname;
            return result;
        }
        #endregion


        public UserFile CreateModel(CreateUploadFileViewModel viewModel, UserFile datamodel = null)
        {
            var fileContent = new byte[viewModel.TempFileInfo.Length];
            using (var reader = viewModel.TempFileInfo.OpenRead())
            {
                reader.Read(fileContent, 0, fileContent.Length);
            }
            var result = datamodel ?? new UserFile();
            result.Name = viewModel.FileName;
            result.ContentType = viewModel.ContentType;
            result.Content = fileContent;
            return result;
        }

        public UploadFileViewModel CreateViewModel(UserFile datamodel)
        {
            if (datamodel == null)
            {
                throw new ArgumentNullException("datamodel");
            }
            return new UploadFileViewModel()
            {
                Id = datamodel.Id,
                Name = datamodel.Name,
                ContentType = datamodel.ContentType,
                ContentSize = datamodel.Content.Length,
                Url = _UrlHelper.Link("GetFileById", new {id = datamodel.Id})
            };
        }
    }
}

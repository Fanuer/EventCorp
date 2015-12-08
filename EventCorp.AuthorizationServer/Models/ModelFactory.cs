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
using EventCorps.Helper.Enums;
using EventCorps.Helper.Models;
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
        Username = datamodel.UserName,
        Email = datamodel.Email,
        DateOfBirth = datamodel.DateOfBirth,
        Surname = datamodel.Surname,
        Forename = datamodel.Forename,
        AvatarId = datamodel.AvatarId,
        City = datamodel.City,
        FavoriteEventType = datamodel.FavoriteEventType,
        GenderType = datamodel.GenderType
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
      result.UserName = model.Username;
      result.Email = model.Email;
      result.Forename = model.Forename;
      result.Surname = model.Surname;
      result.AvatarId = model.AvatarId;
      result.City = model.City;
      result.FavoriteEventType = model.FavoriteEventType;
      result.GenderType = model.GenderType;
      return result;
    }

    public UserFile CreateModel(CreateUploadFileViewModel viewModel, UserFile datamodel = null)
    {
      var fileContent = new byte[viewModel.TempFileInfo.Length];
      using (var reader = viewModel.TempFileInfo.OpenRead())
      {
        reader.Read(fileContent, 0, fileContent.Length);
      }
      var result = datamodel ?? new UserFile() { Id = Guid.NewGuid() };
      result.Name = viewModel.FileName;
      result.ContentType = viewModel.ContentType;
      result.Content = fileContent;
      result.CreatedUTC = viewModel.TempFileInfo.CreationTimeUtc;
      result.Global = viewModel.Global;
      result.UserId = viewModel.Owner.ToString();
      return result;
    }

    public UserFile CreateModel(UploadFileViewModel viewModel, UserFile datamodel)
    {
      var result = datamodel;
      result.Name = viewModel.Name;
      result.ContentType = viewModel.ContentType;
      result.Global = viewModel.Global;
      result.IsTemp = !viewModel.IsPermanent;
      result.CreatedUTC = viewModel.Created;
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
        Global = datamodel.Global,
        Owner = datamodel.Owner,
        IsPermanent = !datamodel.IsTemp,
        Created = datamodel.CreatedUTC,
        Url = _UrlHelper.Link("GetFileById", new { fileId = datamodel.Id })
      };
    }

    #endregion
  }
}

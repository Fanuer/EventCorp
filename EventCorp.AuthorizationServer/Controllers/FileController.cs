using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EventCorp.AuthorizationServer.Models;
using EventCorps.Helper.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.AuthorizationServer.Controllers
{
  
  /// <summary>
  /// Controller to manage files
  /// </summary>
  [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occured")]
  [RoutePrefix("api/file")]
  [Authorize]
  public class FileController : BaseApiController
  {
    
    /// <summary>
    /// Upload a new File
    /// </summary>
    /// <returns></returns>
    /// <response code="201">Created</response>  
    /// <response code="400">Bad request</response> 
    /// <response code="500">Internal Server Error</response>
    [Route("")]
    [HttpPost]
    [SwaggerResponse(HttpStatusCode.Created, Type = typeof(UploadFileViewModel))]
    [SwaggerResponse(HttpStatusCode.BadRequest)]
    public async Task<IHttpActionResult> UploadFile()
    {

      if (!Request.Content.IsMimeMultipartContent())
      {
        return StatusCode(HttpStatusCode.UnsupportedMediaType);
      }

      var provider = GetMultipartProvider();
      // write data in temp file
      var result = await Request.Content.ReadAsMultipartAsync(provider);

      var file = result.FileData.FirstOrDefault();
      if (file == null)
      {
        return BadRequest("No File was sent");
      }

      var viewmodel = CreateUploadFileViewModel(file, provider);

      var model = AppModelFactory.CreateModel(viewmodel);
      await this.AppRepository.Files.AddAsync(model);
      var resultmodel = AppModelFactory.CreateViewModel(model);

      // file content is saved in DB and can be deleted from hard drive
      viewmodel.TempFileInfo.Delete();
      DeleteOldTempFiles();

      return CreatedAtRoute("GetFileById", new { id = resultmodel.Id }, resultmodel);
    }

    /// <summary>
    /// Gets metadata for file
    /// </summary>
    /// <param name="fileId">id of the file</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{fileId:guid}", Name = "GetFileById")]
    [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UploadFileViewModel))]
    [SwaggerResponse(HttpStatusCode.NotFound)]
    public async Task<IHttpActionResult> GetFileData([FromUri] Guid fileId)
    {
      var file = await AppRepository.Files.FindAsync(fileId);

      if (file == null || (!file.Global && !CheckOwner(file.Owner)))
      {
        return NotFound();
      }

      return Ok(AppModelFactory.CreateViewModel(file));
    }

    /// <summary>
    /// Return the file content of a given fileId
    /// </summary>
    /// <param name="fileId">id of a file</param>
    /// <returns></returns>
    [Route("{fileId:guid}/content")]
    [AllowAnonymous]
    [HttpGet]
    [SwaggerResponse(HttpStatusCode.OK, Type = typeof(byte[]))]
    [SwaggerResponse(HttpStatusCode.NotFound)]
    public async Task<IHttpActionResult> DownloadFile([FromUri] Guid fileId)
    {
      var file = await AppRepository.Files.FindAsync(fileId);
      if (file == null)
      {
        return NotFound();
      }

      var result = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new StreamContent(new MemoryStream(file.Content))
      };
      result.Content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
      result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
      {
        FileName = file.Name
      };

      return ResponseMessage(result);
    }

    
    /// <summary>
    /// Changes if the file is a temporary or a persistant file
    /// </summary>
    /// <param name="fileId">id of a file</param>
    /// <param name="state">true: file is persistent, false: temaporary file</param>
    /// <returns></returns>
    [HttpPut]
    [Route("{fileId:guid}/state/{state}")]
    [SwaggerResponse(HttpStatusCode.NoContent)]
    [SwaggerResponse(HttpStatusCode.BadRequest)]
    [SwaggerResponse(HttpStatusCode.NotFound)]
    public async Task<IHttpActionResult> ChangeFileState([FromUri]Guid fileId, bool state)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var file = await AppRepository.Files.FindAsync(fileId);
      if (file == null)
      {
        return NotFound();
      }

      if (!file.Global && !CheckOwner(file.Owner))
      {
        ModelState.AddModelError("", "The status of a private file can be changed only by its owner");
      }
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      file.IsTemp = !state;
      await AppRepository.Files.UpdateAsync(file);
      return StatusCode(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Updates an existing file. Only own files can be updated
    /// </summary>
    /// <param name="fileId">Id of the file to update</param>
    /// <param name="model">new file data</param>
    [Route("{fileId:guid}")]
    [HttpPut]
    [SwaggerResponse(HttpStatusCode.NoContent)]
    [SwaggerResponse(HttpStatusCode.BadRequest)]
    [SwaggerResponse(HttpStatusCode.NotFound)]
    public async Task<IHttpActionResult> ChangeFile([FromUri] Guid fileId, [FromBody] UploadFileViewModel model)
    {
      if (!fileId.Equals(model.Id))
      {
        ModelState.AddModelError("id", "The given id have to be the same as in the model");
      }
      if (!CheckOwner(model.Owner))
      {
        ModelState.AddModelError("Owner", "A private file can be changed only by its owner");
      }
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var exists = await this.AppRepository.Files.ExistsAsync(fileId);

      try
      {
        var orig = await this.AppRepository.Files.FindAsync(fileId);
        orig = this.AppModelFactory.CreateModel(model, orig);
        await this.AppRepository.Files.UpdateAsync(orig);
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!exists)
        {
          return NotFound();
        }
        throw;
      }

      return StatusCode(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Deletes a file. Only own files can be deleted
    /// </summary>
    /// <param name="fileId"></param>
    /// <returns></returns>
    [Route("{fileId:guid}")]
    [HttpDelete]
    [SwaggerResponse(HttpStatusCode.OK)]
    [SwaggerResponse(HttpStatusCode.NotFound)]
    public async Task<IHttpActionResult> DeleteFile([FromUri] Guid fileId)
    {
      var file = await this.AppRepository.Files.FindAsync(fileId);
      if (file== null || !CheckOwner(file.Owner))
      {
        return NotFound();
      }
      await AppRepository.Files.RemoveAsync(file);
      return Ok();
    }
    
    /// <summary>
    /// Extracts Request FormatData as a strongly typed model 
    /// </summary>
    /// <typeparam name="T">Result Type</typeparam>
    /// <param name="provider"></param>
    /// <returns></returns>
    private T GetFormData<T>(MultipartFormDataStreamProvider provider)
    {
      T result = default(T);
      if (provider.FormData.HasKeys())
      {
        var unescapedFormData = Uri.UnescapeDataString(provider.FormData.GetValues(0).FirstOrDefault() ?? String.Empty);
        if (!String.IsNullOrEmpty(unescapedFormData))
        {
          result = JsonConvert.DeserializeObject<T>(unescapedFormData);
        }
      }

      return result;
    }

    /// <summary>
    /// GetFilename from multipart provider
    /// </summary>
    /// <param name="fileData"></param>
    /// <returns></returns>
    private string GetDeserializedFileName(MultipartFileData fileData)
    {
      var fileName = fileData.Headers.ContentDisposition.FileName;
      return JsonConvert.DeserializeObject(fileName).ToString();
    }

    /// <summary>
    /// Creates a Multipartprovider to save uploaded files temporary
    /// </summary>
    /// <returns></returns>
    private MultipartFormDataStreamProvider GetMultipartProvider()
    {
      var uploadFolder = "~/App_Data/Tmp/FileUploads"; // you could put this to web.config
      var root = HttpContext.Current.Server.MapPath(uploadFolder);
      Directory.CreateDirectory(root);
      return new MultipartFormDataStreamProvider(root);
    }

    /// <summary>
    /// Deletes old temp files
    /// </summary>
    private void DeleteOldTempFiles()
    {
      var uploadFolder = "~/App_Data/Tmp/FileUploads";
      var root = HttpContext.Current.Server.MapPath(uploadFolder);
      var dirInfo = new DirectoryInfo(root);
      var files = dirInfo.GetFiles().Where(x => x.CreationTime < DateTime.Now.AddMinutes(-60));
      foreach (var file in files)
      {
        file.Delete();
      }
    }

    private CreateUploadFileViewModel CreateUploadFileViewModel(MultipartFileData file, MultipartFormDataStreamProvider provider)
    {
      var contentType = file.Headers.ContentType.MediaType;
      var originalFileName = GetDeserializedFileName(file);

      var global = false;
      var formdata = GetFormData<UploadedFileFormModel>(provider);
      if (formdata != null)
      {
        global = formdata.Global;
      }

      var viewmodel = new CreateUploadFileViewModel
      {
        ContentType = contentType,
        TempFileInfo = new FileInfo(file.LocalFileName),
        FileName = originalFileName,
        Owner = new Guid(User.Identity.GetUserId()),
        Global = global
      };
      return viewmodel;
    }

    private bool CheckOwner(Guid owner)
    {
      return this.CurrentUserId.Equals(owner.ToString());
    }
    
  }

}

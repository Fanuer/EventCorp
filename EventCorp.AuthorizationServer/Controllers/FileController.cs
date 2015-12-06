using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.AuthorizationServer.Controllers
{
    /// <summary>
    /// Controller to manage files
    /// </summary>
    //[SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occured")]
    //[Authorize(Roles = "Admin")]
    [RoutePrefix("api/file")]
    public class FileController : BaseApiController
    {
        /// <summary>
        /// Uploads a new File
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
            UploadFileViewModel resultmodel = null;
            try
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

                var fileData = result.FileData.First();
                var contentType = fileData.Headers.ContentType.MediaType;
                var originalFileName = GetDeserializedFileName(result.FileData.First());

                var viewmodel = new CreateUploadFileViewModel
                {
                    ContentType = contentType,
                    TempFileInfo = new FileInfo(result.FileData.First().LocalFileName),
                    FileName = originalFileName
                };

                var model = AppModelFactory.CreateModel(viewmodel);
                await this.AppRepository.Files.AddAsync(model);
                resultmodel = AppModelFactory.CreateViewModel(model);

                // file content is saved in DB and can be deleted from hard drive
                viewmodel.TempFileInfo.Delete();
                DeleteOldTempFiles();
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
            return CreatedAtRoute("GetFileById", new { id = resultmodel.Id }, resultmodel);
        }

        /// <summary>
        /// Return the file content of a given fileId
        /// </summary>
        /// <param name="fileId">id of a file</param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(byte[]))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("{fileId:guid}", Name = "GetFileById")]
        [HttpGet]
        public async Task<IHttpActionResult> DownloadFile(string fileId)
        {
            var file = await AppRepository.Files.FindAsync(new Guid(fileId));
            if (file == null)
            {
                return NotFound();
            }
            
            var streamContent = new StreamContent(new MemoryStream(file.Content));
            var result = Ok(streamContent);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = file.Name
            };
            return result;
        }

        /// <summary>
        /// Changes if the file is a temporary or a persistant file.
        /// </summary>
        /// <param name="fileId">id of a file</param>
        /// <param name="state">true: file is persistent, false: temaporary file</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{fileId:guid}")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> ChangeFileState([FromUri]Guid fileId, [FromBody]bool state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var file = await AppRepository.Files.FindAsync(fileId);
            if (file== null)
            {
                return NotFound();
            }

            file.IsTemp = !state;
            await AppRepository.Files.UpdateAsync(file);
            return StatusCode(HttpStatusCode.NoContent);
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

        private void DeleteOldTempFiles()
        {
            var uploadFolder = "~/App_Data/Tmp/FileUploads"; 
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            var dirInfo = new DirectoryInfo(root);
            var files = dirInfo.GetFiles("*.*").Where(x => x.CreationTime < DateTime.Now.AddMinutes(-60));
            foreach (var file in files)
            {
                file.Delete();
            }
        }

    }
}

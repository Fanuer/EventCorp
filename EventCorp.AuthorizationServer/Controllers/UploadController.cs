

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.AuthorizationServer.Controllers
{
    /// <summary>
    /// Controler to manage uploaded files
    /// </summary>
    [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occured")]
    //[Authorize(Roles = "Admin")]
    [RoutePrefix("api/upload")]
    public class UploadController:BaseApiController
    {
        /// <summary>
        /// Creates a new File
        /// </summary>
        /// <param name="model">role data</param>
        /// <returns></returns>
        [Route("multipart")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created, Type = typeof(Guid))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public async Task<IHttpActionResult> PostFormDataAsMultiPart()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            /*
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                }
                return Ok();
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }*/
            return Ok();
        }
        [Route("requestfile")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created, Type = typeof(Guid))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public async Task<IHttpActionResult> PostFormDataAsRequestFiles()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection  
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
                if (httpPostedFile != null)
                {
                    /*var imgupload = new UploadFile();
                    int length = httpPostedFile.ContentLength;
                    imgupload.imagedata = new byte[length]; //get imagedata  
                    httpPostedFile.InputStream.Read(imgupload.imagedata, 0, length);
                    imgupload.imagename = Path.GetFileName(httpPostedFile.FileName);
                    db.fileUpload.Add(imgupload);
                    db.SaveChanges();
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);
                    // Save the uploaded file to "UploadedFiles" folder  
                    httpPostedFile.SaveAs(fileSavePath);*/
                    return Ok("Image Uploaded");
                }
            }
            return Ok("Image is not Uploaded");
        }
    }
}

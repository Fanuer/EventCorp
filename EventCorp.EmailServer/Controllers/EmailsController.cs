using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using EventCorp.Clients;
using EventCorps.Helper;
using EventCorps.Helper.Enums;
using EventCorps.Helper.Models;
using Microsoft.AspNet.Identity.Owin;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.EmailServer.Controllers
{
    /// <summary>
    /// Sends Emails to EventCorps users
    /// </summary>
    [Authorize]
    [RoutePrefix("api/emails")]
    public class EmailsController : ApiController
    {
        #region Fields
        private SimpleManagementSession _session = null;
        #endregion

        #region Methods
        /// <summary>
        /// Send a Mail
        /// </summary>
        /// <param name="model">email data</param>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<MailMessage>))]
        public async Task<IHttpActionResult> SendMail(EmailModel model)
        {
            List<MailMessage> result = new List<MailMessage>();

            try
            {
                var sendTo = await GetMailReceivers(model.SendTo);
                var messagedata = GetEmailContent(model);

            }
            catch (Exception e)
            {
                return InternalServerError(new Exception("Error on sending Mail", e));
            }
            return Ok(result);
        }

        private EmailContent GetEmailContent(EmailModel model)
        {
            var emailContent = new EmailContent();
            switch (model.EmailType)
            {
                case EmailTypes.RegisteredSuccessfully:
                    emailContent.Header = "Registrierung bei EventCorp";
                    emailContent.Body = "Vielen Dank, dass sich sich für uns entschieden haben"; 
                    break;
                case EmailTypes.Subscribed:

                    break;
                case EmailTypes.Unsubscribed:
                    break;
                case EmailTypes.ChangedUserData:
                    break;
                case EmailTypes.ChangedPassword:
                    break;
                case EmailTypes.UserRegistered:
                    break;
                case EmailTypes.EventCreated:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return emailContent;
        }

        private async Task<List<string>> GetMailReceivers(SendToTypes sendToType)
        {
            var sendTo = new List<string>();
            switch (sendToType)
            {
                case SendToTypes.User:
                    var userEmail = (await AppManagementSession.Users.GetCurrentUserAsync(this.BearerToken)).Email;
                    sendTo.Add(userEmail);
                    break;
                case SendToTypes.Admins:
                    sendTo = (await AppManagementSession.Users.GetAdminEmailAddressesAsync(this.BearerToken)).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return sendTo;
        }



        #endregion

        #region Properties
        /// <summary>
        /// Repository
        /// </summary>
        private SimpleManagementSession AppManagementSession
        {
            get { return this._session ?? Request.GetOwinContext().Get<SimpleManagementSession>(); }
        }

        private string BearerToken
        {
            get
            {
                return this.Request.Headers.Authorization.Parameter;
            }
        }

        #endregion

        #region nested

        private class EmailContent
        {
            public string Header { get; set; }
            public string Body { get; set; }
        }
        #endregion
    }
}

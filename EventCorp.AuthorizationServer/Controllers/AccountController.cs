using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using EventCorp.AuthorizationServer.Entites;
using EventCorp.AuthorizationServer.Interfaces.Repositories;
using EventCorp.AuthorizationServer.Models;
using EventCorps.Helper.Models;
using Microsoft.AspNet.Identity;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.AuthorizationServer.Controllers
{
    /// <summary>
    /// Handles User-based Actions
    /// </summary>
    [Authorize]
    [RoutePrefix("api/accounts")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occured")]
    public class AccountController : BaseApiController
    {
        /// <summary>
        /// Method to prove the Server's availability
        /// </summary>
        [ResponseType(typeof(void))]
        [Route("ping")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Ping()
        {
            var result = new { timestamp = DateTime.Now };
            return this.Ok(result);
        }

        #region Users
        /// <summary>
        /// Gets all application Users
        /// </summary>
        [Route("users")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<UserModel>))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
        [Authorize(Roles = "Admin")]
        public IQueryable<UserModel> GetUsers()
        {
            return this.AppUserManager.Users.ToList().Select(u => this.AppModelFactory.CreateViewModel(u)).AsQueryable();
        }

        /// <summary>
        /// Get a user by its guid
        /// </summary>
        /// <param name="id">User's guid</param>
        [Route("users/{id:guid}", Name = "GetUserById")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserModel))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            var user = await this.AppUserManager.FindByIdAsync(id);

            if (user != null)
            {
                return Ok(this.AppModelFactory.CreateViewModel(user));
            }

            return NotFound();

        }

        /// <summary>
        /// Get User by Username
        /// </summary>
        /// <param name="username">username to search for</param>
        [Route("users/{username}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserModel))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);
            if (user != null)
            {
                return Ok(this.AppModelFactory.CreateViewModel(user));
            }
            return NotFound();
        }
        /// <summary>
        /// Returns the current users Information
        /// </summary>
        [Route("currentUser")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserModel))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> GetCurrentUser()
        {
            var currentUserId = User.Identity.GetUserId();
            var user = await this.AppUserManager.FindByIdAsync(currentUserId);

            if (user != null)
            {
                return Ok(this.AppModelFactory.CreateViewModel(user));
            }
            return NotFound();
        }

        [Route("currentUser/roles")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserModel))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> GetUserRoles()
        {
            var user = await this.AppUserManager.FindByIdAsync(CurrentUserId);
            if (user != null)
            {
                var roleIds = user.Roles.Select(x => x.RoleId);
                return Ok(AppRoleManager.Roles.Where(x => roleIds.Contains(x.Id)).Select(x => x.Name));
            }
            return NotFound();
        }


        /// <summary>
        /// Updates user data
        /// </summary>
        /// <param name="model">user data</param>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("currentUser")]
        [HttpPut]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public async Task<IHttpActionResult> UpdateCurrentUser(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.Identity.GetUserId();
            var user = await this.AppUserManager.FindByIdAsync(currentUserId);
            user = this.AppModelFactory.CreateModel(model, user);
            var result = await this.AppUserManager.UpdateAsync(user);
            return !result.Succeeded ? GetErrorResult(result) : StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// User can register to the Application
        /// </summary>
        /// <param name="createUserModel">new User</param>
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        [Route("users")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Register(CreateUserModel createUserModel)
        {
            // validate model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = AppModelFactory.CreateModel(createUserModel);
            var addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            var locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
            return Created(locationHeader, AppModelFactory.CreateViewModel(user));
        }

        /// <summary>
        /// User can change its password
        /// </summary>
        /// <param name="model">Data to change a password</param>
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("users/password")]
        [HttpPut]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Admin can delete User
        /// </summary>
        /// <param name="id">Id of the user to delete</param>
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("users/{id:guid}")]
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var appUser = await this.AppUserManager.FindByIdAsync(id);
            if (appUser != null)
            {
                var result = await this.AppUserManager.DeleteAsync(appUser);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// THIS METHOD IS FOR SWAGGER USE ONLY. 
        /// Hack to login an user and to receive an access token. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Invoke the "token" OWIN service to perform the login: /api/token
            // Ugly hack: I use a server-side HTTP POST because I cannot directly invoke the service (it is deeply hidden in the OAuthAuthorizationServerHandler class)
            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + "/oauth2/token";
            using (var client = new HttpClient())
            {
                var requestParams = new Dictionary<string, string>()
                {
                    {"grant_type", "password"},
                    {"username", login.Username},
                    {"password", login.Password},
                    {"client_id", "0dd23c1d3ea848a2943fa8a250e0b2ad"}
                };

                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var responseCode = tokenServiceResponse.StatusCode;
                var responseMsg = new HttpResponseMessage(responseCode)
                {
                    Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                };
                return ResponseMessage(responseMsg);
            }
        }
        #endregion

        #region Roles
        /// <summary>
        /// Assigned the user to the given roles
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="rolesToAssign">Roles to assign to the user</param>
        [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Authorize(Roles = "Admin")]
        [Route("users/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            var appUser = await this.AppUserManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);
            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Any())
            {
                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }
        #endregion
    }
}

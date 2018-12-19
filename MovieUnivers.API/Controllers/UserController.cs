using System.Net;
using System.Web.Http;
using MovieUniverse.Abstract.ApiModels;
using MovieUniverse.Abstract.Entities.UserEntityes;
using MovieUniverse.Abstract.Models;
using MovieUniverse.Abstract.Services.UserServices;
using MovieUniverse.API.Constants;
using MovieUniverse.API.Filters;
using MovieUniverse.DAL.DataAccess.Repositories.UserRepositories;

namespace MovieUniverse.API.Controllers
{
    [CustomAuthorize]
    public class UserController :ApiControllerBase
    {
        private readonly AppUserRepository _userRepository = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(AppUserRepository)) as AppUserRepository;
        private IAppUserService UserService { get; set; }


        [HttpGet]
        [Route("api/users/notifications")]
        [ValidationFilter]
        public IHttpActionResult GetNotifications()
        {
            Response.Message = Messages.SucessMessage;
            Response.StatusCode = HttpStatusCode.OK;
            
            return Ok(Response);
        }

        [HttpPost]
        [Route("api/users/")]
        [ValidationFilter]
        [AllowAnonymous]
        public IHttpActionResult Register([FromBody]UserModel userModel)
        {
            Response.Message = Messages.SucessMessage;
            Response.StatusCode = HttpStatusCode.OK;

            AppUser user = new AppUser()
            {
                Email = userModel.Email,
                Password = userModel.Password,
                IsPublicUser = true,
                Name = userModel.Name,
            };
            UserService.RegisterUser(user);
            Response.Response = null;
            return Ok(Response);
        }

        [HttpGet]
        [Route("api/users/{id:long}")]
        [AllowAnonymous]
        public IHttpActionResult GetById([FromUri] long id)
        {
            Response.Response = UserService.GetById(UserSession.Id,id);
            Response.Message = Messages.SucessMessage;
            Response.StatusCode = HttpStatusCode.OK;
            return Ok(Response);
        }

        [HttpPost]
        [Route("api/users/token")]
        [AllowAnonymous]
        public IHttpActionResult LoginUser([FromBody] UserLoginModel userLogin)
        {
            var response = new ResponseModel()
            {
                StatusCode = HttpStatusCode.OK,
                Message = Messages.SucessMessage,
                Response = UserService.Login(userLogin),
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("api/users/oauth")]
        [AllowAnonymous]
        public IHttpActionResult ExternalLogin([FromBody] OAuthUserLoginModel userLoginModel)
        {
            Response.Message = Messages.SucessMessage;
            Response.StatusCode = HttpStatusCode.OK;
            Response.Response = UserService.OAuthLogin(userLoginModel);
            
            return Ok(Response);
        }

        
        [HttpGet]
        [Route("api/users/verfication")]
        [AllowAnonymous]
        public IHttpActionResult ConfirmUser([FromUri] AccountActivationModel model)
        {
            Response.Message = Messages.SucessMessage;
            Response.Response = null;
            Response.StatusCode = HttpStatusCode.OK;
            UserService.ConfirmUser(model);
            return Ok(Response);
        }
        
    }
}

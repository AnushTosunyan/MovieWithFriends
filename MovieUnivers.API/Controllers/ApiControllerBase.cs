using System;
using System.Collections.Generic;

using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MovieUniverse.Abstract;
using MovieUniverse.Abstract.ApiModels;
using MovieUniverse.Abstract.Entities;
using MovieUniverse.Abstract.Filers;
using MovieUniverse.API.Constants;
using MovieUniverse.API.Filters;
using MovieUniverse.API.Models;
using MovieUniverse.API.Ninject;
using MovieUniverse.Facade.Facade;
using MovieUniverse.Facade.Models.RequsetDTO;
using MovieUniverse.Facade.Models.RequsetDTO.Filters;
using MovieUniverse.Services.Factory;
using MovieUniverse.Services.ServiceImpl;

namespace MovieUniverse.API.Controllers
{
    public class ApiControllerBase : ApiController
    {
        public UserSessionModel UserSession { get; set; }
        protected ResponseModel Response;

        public ApiControllerBase() : base()
        {
            Response = new ResponseModel() {Message = "Not Implemented Method"};
        }
        
    }
}

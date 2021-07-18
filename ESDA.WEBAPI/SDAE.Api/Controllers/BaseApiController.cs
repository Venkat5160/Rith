using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SDAE.Common;
using SDAE.Data.Model;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechRAQ.Common.LoggingService.Serilog.Interfaces.Service;

namespace SDAE.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = IdentityServerConstants.LocalApi.AuthenticationScheme)]
    public class BaseApiController : ControllerBase
    {
        protected readonly ILoggingService _loggingService;
        protected readonly IMediator _mediator;

        public BaseApiController()
        {
        }

        public BaseApiController(ILoggingService loggingService) : this()
        {
            _loggingService = loggingService;
        }


        public BaseApiController(IMediator mediator, ILoggingService loggingService) : this(loggingService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected ActionResult InternalServerError()
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        protected ActionResult InternalServerError(object value)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, value);
        }

        protected ActionResult NoDataFound(object value)
        {
            return StatusCode((int)HttpStatusCode.NotFound, value);
        }

        public UserIdentityData UserData
        {
            get
            {
                UserIdentityData curUserData = null;
                if (User.Identity.IsAuthenticated)
                {
                    curUserData = new UserIdentityData()
                    {
                        UserName = User.Claims.FirstOrDefault(o => o.Type == "preferred_username").Value?.ToString(),
                        UserId = User.Claims.FirstOrDefault(o => o.Type == "sub").Value?.ToString(),
                        RoleId = ((int)Enum.Parse(typeof(RolesEnum), User.Claims.FirstOrDefault(o => o.Type.Contains("role")).Value?.ToString().Replace(" ", ""))).ToString(),
                        SessionId = User.Claims.FirstOrDefault(o => o.Type == "SessionId").Value?.ToString(),
                        FirtsName = User.Claims.FirstOrDefault(o => o.Type == "firstname").Value?.ToString()
                    };
                }
                return curUserData;
            }
        }
    }
}

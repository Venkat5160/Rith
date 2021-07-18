using System;
using System.Threading.Tasks;
using SDAE.Api.Application.Account.Commands;
using SDAE.Api.Application.Account.Models;
using SDAE.Api.Application.Account.Queries;
using SDAE.Common;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog;
using static IdentityModel.OidcConstants;

namespace SDAE.Api.Services
{
    public class ResourceOwnerPasswordValidator<TUser> : IResourceOwnerPasswordValidator
        where TUser : class
    {
        private readonly SignInManager<TUser> _signInManager;
        private IEventService _events;
        private readonly UserManager<TUser> _userManager;
        private readonly ILogger<ResourceOwnerPasswordValidator<TUser>> _logger;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOwnerPasswordValidator{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="events">The events.</param>
        /// <param name="logger">The logger.</param>
        public ResourceOwnerPasswordValidator(
            UserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            IEventService events,
            ILogger<ResourceOwnerPasswordValidator<TUser>> logger,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
            _logger = logger;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetIp()
        {
            string ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip == "::1")
                ip = "127.0.0.1";

            return ip;
        }
        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code", "CAC001:ConfigureAwaitChecker", Justification = "<Pending>")]
        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            Guid sessionId = Guid.NewGuid();
            try
            {
                var user = await _userManager.FindByNameAsync(context.UserName);
                if (user != null)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    CommandResult commandResult = await _mediator.Send(new GetUsersQuery() { UserId = userId }).ConfigureAwait(false);
                    bool isAdmin = Convert.ToBoolean(context.Request.Raw.Get("IsAdmin"));

                    // TODO : Need to check below code later
                    //if (!string.IsNullOrEmpty(commandResult.Result[0].Role))
                    //{
                    //	if (isAdmin == false && commandResult.Result[0].Role == "Admin")
                    //	{
                    //		Log.Information("Authentication failed for username: {username}, reason: invalid grant", context.UserName);
                    //		await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "Invalid grant", interactive: false));
                    //		await UserLogs(context, sessionId, userId);
                    //		context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Access denied");
                    //		return;
                    //	}
                    //}
                    if (commandResult.Result != null && commandResult.Result.StatusTypeId != (byte)StatusTypesEnum.Active)
                    {
                        Log.Information("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "Deactive User", interactive: false));
                        await UserLogs(context, sessionId, userId);
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
                        return;
                    }
                    var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
                    if (result.Succeeded)
                    {
                        Log.Information("Credentials validated for username: {username}", context.UserName);
                        await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, userId, context.UserName, interactive: false));
                        context.Result = new GrantValidationResult(userId, AuthenticationMethods.Password);
                        return;
                    }
                    else if (result.IsLockedOut)
                    {
                        Log.Information("Authentication failed for username: {username}, reason: locked out", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out", interactive: false));
                        await UserLogs(context, sessionId, userId);
                    }
                    else if (result.IsNotAllowed)
                    {
                        Log.Information("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", interactive: false));
                        await UserLogs(context, sessionId, userId);
                    }
                    else
                    {
                        Log.Information("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", interactive: false));
                        await UserLogs(context, sessionId, userId);
                    }
                }
                else
                {
                    await UserLogs(context, sessionId, string.Empty);
                    Log.Information("No user found matching username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                }

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
            }
            catch (Exception ex)
            {
            }
        }
        private async Task UserLogs(ResourceOwnerPasswordValidationContext context, Guid sessionId, string userId)
        {
            UserLogsDto userLog = new UserLogsDto();
            {
                userLog.UserName = context.UserName;
                userLog.UserId = userId;
                userLog.ClientIp = GetIp();
                userLog.LogonTime = Formatter.GetUtcDateTime();
                userLog.SessionId = sessionId;
                userLog.DeviceModel = context.Request.Raw.Get("DeviceModel");
                userLog.DeviceToken = context.Request.Raw.Get("DeviceToken");
                userLog.LoginDeviceTypeId = Convert.ToInt16(context.Request.Raw.Get("LoginDeviceTypeId"));
                userLog.LoginStatusTypeId = (int)LoginStatusTypesEnum.InvalidCredentials;
            }
            await _mediator.Send(new UpsertUserLogsCommand() { UserLogsDto = userLog }).ConfigureAwait(false);
        }

    }
}

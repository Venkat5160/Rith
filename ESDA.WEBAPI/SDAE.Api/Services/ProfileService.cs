using SDAE.Api.Application.Account.Commands;
using SDAE.Api.Application.Account.Models;
using SDAE.Api.Models;
using SDAE.Common;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SDAE.Api.Services
{
    /// <summary>
    /// IProfileService to integrate with ASP.NET Identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <seealso cref="IdentityServer4.Services.IProfileService" />
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProfileService(UserManager<ApplicationUser> userManager, IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
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
        async public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(subjectId).ConfigureAwait(false);
            if (user == null)throw new ArgumentException("Invalid subject identifier");
            var role = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var claims = GetClaimsFromUser(user, role[0]);
            var claims1 = GetClaimsFromUser(user, "");
            Guid sessionId = Guid.Parse(claims.Where(x => x.Type == "SessionId").FirstOrDefault().Value);
            UserLogsDto userLog = new UserLogsDto();
            {
                userLog.UserName = user.UserName;
                userLog.UserId = user.Id;
                userLog.ClientIp = GetIp();
                userLog.LogonTime = Formatter.GetUtcDateTime();
                userLog.SessionId = sessionId;
                userLog.DeviceModel = context.ValidatedRequest.Raw.Get("DeviceModel");
                userLog.DeviceToken = context.ValidatedRequest.Raw.Get("DeviceToken");
                userLog.LoginDeviceTypeId = Convert.ToInt16(context.ValidatedRequest.Raw.Get("LoginDeviceTypeId"));
                userLog.LoginStatusTypeId = (int)LoginStatusTypesEnum.SuccessfulLogin;
            }
            CommandResult commandResult = await _mediator.Send(new UpsertUserLogsCommand() { UserLogsDto = userLog }).ConfigureAwait(false);
            context.IssuedClaims = claims.ToList();
        }

        async public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(subjectId).ConfigureAwait(false);
            context.IsActive = false;
            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await _userManager.GetSecurityStampAsync(user).ConfigureAwait(false);
                        if (db_security_stamp != security_stamp)
                            return;
                    }
                }
                context.IsActive = !user.LockoutEnabled || !user.LockoutEnd.HasValue || user.LockoutEnd <= DateTime.Now;
            }
        }

        private IEnumerable<Claim> GetClaimsFromUser(ApplicationUser user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };
            if (!string.IsNullOrWhiteSpace(user.FirstName))
                claims.Add(new Claim("firstname", user.FirstName));
            if (!string.IsNullOrWhiteSpace(user.FirstName))
                claims.Add(new Claim("SessionId", Guid.NewGuid().ToString()));
            return claims;
        }
        private string GetRawContent(ProfileDataRequestContext request, string key)
        {
            try
            {
                return request.ValidatedRequest.Raw[key];
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

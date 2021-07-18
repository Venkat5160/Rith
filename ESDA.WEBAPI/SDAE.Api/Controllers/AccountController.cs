using SDAE.Api.Application.Account.Commands;
using SDAE.Api.Application.Account.Models;
using SDAE.Api.Models;
using SDAE.Common;
using SDAE.Data.Model.Account;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechRAQ.Common.LoggingService.Serilog.Interfaces.Service;
using SDAE.Api.Application.Account.Queries;

namespace SDAE.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IOptions<NotificationSettings> _appSettings;
        public AccountController(IMediator mediator, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
             ILoggingService loggingService) : base(mediator, loggingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("SaveUser")]
        public async Task<ActionResult<CommandResult>> Register([FromBody] UserDto userDto)
        {
            CommandResult commandResult = new CommandResult();
            commandResult.StatusCode = 200;
            try
            {
                var user = new ApplicationUser
                {
                    UserName = userDto.Email,
                    Email = userDto.Email,
                    LastName = userDto.LastName,
                    FirstName = userDto.FirstName,
                    PhoneNumber = userDto.Contact,
                    CreatedBy = UserData.UserId,
                    CreatedOn = Formatter.GetUtcDateTime(),
                    StatusTypeId = (byte)StatusTypesEnum.Active
                };
                var result = await _userManager.CreateAsync(user, userDto.Password).ConfigureAwait(false);
                if (result.Errors.Count() > 0)
                {
                    AddErrors(result);
                    if (result.Errors.FirstOrDefault().Code.ToLower().Contains("duplicateusername"))
                    {
                        commandResult.Message = "Email already exists";
                        commandResult.StatusCode = 300;
                        return Ok(commandResult);
                    }
                    commandResult.Message = Constraints.ErrorResponse;
                    return InternalServerError(commandResult);
                }
                string callbackUrl = string.Empty;
                commandResult = await _mediator.Send(new UpsertRoleCommand() { UserId = user.Id, RoleId = userDto.RoleId }).ConfigureAwait(false);
                commandResult.Message = Constraints.AddResponse;
                //userDto.Role = ((RolesEnum)Convert.ToInt32(userDto.RoleId)).ToString();
                if (result.Succeeded)
                {
                    commandResult.StatusCode = 200;

                    //TO DO: Need to very the user upon the creation.
                    //callbackUrl = await GenrateTokenAndSendEmail((int)MailSentTypes.Registration, userDto, user).ConfigureAwait(false);
                }
                return Ok(commandResult);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error occurred while user logging out");
                commandResult.Message = Constraints.ErrorResponse;
            }
            return InternalServerError(commandResult);
        }
        [HttpGet("GetUserById")]
        public async Task<ActionResult<CommandResult>> GetUserById(string userId)
        {
            try
            {
                CommandResult cmd = await _mediator.Send(new GetUsersQuery() { UserId = userId }).ConfigureAwait(false);
                return Ok(cmd);
            }
            catch (Exception ex)
            {
                CommandResult cmd = new CommandResult
                {
                    StatusCode = (int)StatusCodesEnum.Error,
                    Message = Constraints.ErrorResponse,
                    Successful = false,
                };
                return cmd;
            }
        }
        [HttpGet("GetUserDetails")]
        public async Task<ActionResult<CommandResult>> GetUserDetails()
        {
            try
            {
                CommandResult cmd = await _mediator.Send(new GetUsersQuery() { UserId = UserData.UserId }).ConfigureAwait(false);
                return Ok(cmd);
            }
            catch (Exception ex)
            {
                CommandResult cmd = new CommandResult
                {
                    StatusCode = (int)StatusCodesEnum.Error,
                    Message = Constraints.ErrorResponse,
                    Successful = false,
                };
                return cmd;
            }
        }
        [HttpPost("UpdateUser")]
        public async Task<ActionResult<CommandResult>> UpdateUser([FromBody] UserDto userDto)
        {
            CommandResult commandResult = new CommandResult();
            commandResult.StatusCode = 200;
            try
            {
                var userDetails = await _userManager.FindByIdAsync(userDto.UserId).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(userDto.UserId) && userDetails != null)
                {
                    userDetails.LastName = userDto.LastName;
                    userDetails.FirstName = userDto.FirstName;
                    userDetails.PhoneNumber = userDto.Contact;
                    userDetails.UpdatedBy = UserData.UserId;
                    userDetails.UpdatedOn = Formatter.GetUtcDateTime();
                    var result = await _userManager.UpdateAsync(userDetails).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        commandResult.Message = Constraints.UpdateResponse;
                        commandResult.Successful = true;
                        commandResult.StatusCode = 200;
                    }
                }
                else
                {
                    commandResult.Message = Constraints.ErrorResponse;
                }
                return Ok(commandResult);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error occurred while user logging out");
                commandResult.Message = Constraints.ErrorResponse;
            }
            return InternalServerError(commandResult);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        [HttpGet]
        [Route("Logout")]
        public async Task<ActionResult<CommandResult>> Logout()
        {
            CommandResult commandResult;
            try
            {
                await _signInManager.SignOutAsync().ConfigureAwait(false);
                string userId = UserData.UserId;
                Guid sessionId = Guid.Parse(UserData.SessionId);
                Log.Information($"user logging out started {userId}");
                commandResult = await _mediator.Send(new UpsertUserLogsCommand() { UserLogsDto = new UserLogsDto() { UserId = userId, SessionId = sessionId, } }).ConfigureAwait(false);
                Log.Information($"user logging out completed {userId}");
                return Ok(commandResult);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error occurred while user logging out");
                commandResult = new CommandResult()
                {
                    Message = Constraints.ErrorResponse
                };
            };
            return InternalServerError(commandResult);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("UpdatePassword")]
        public async Task<ActionResult<CommandResult>> UpdatePassword(PasswordDto pwd)
        {
            CommandResult commandResult = new CommandResult();
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(pwd.UserId).ConfigureAwait(false);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                IdentityResult result = await _userManager.ResetPasswordAsync(user, token, pwd.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    commandResult.StatusCode = 200;
                    commandResult.Message = "Password updated successfully. Please login with updated credentials";
                    Log.Information("Password updated for User:" + pwd.UserId);
                    return Ok(commandResult);
                }
                else
                {
                    Log.Information("Error occurred ", result.Errors.FirstOrDefault().Code);
                    commandResult.Message = result.Errors.FirstOrDefault().Code;
                    commandResult.StatusCode = 300;
                    return Ok(commandResult);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while update the password for user:" + pwd.UserId);
                commandResult.StatusCode = 500;
                return InternalServerError(commandResult);
            }
        }

        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        public async Task<ActionResult<CommandResult>> DeleteUser(string userId)
        {
            CommandResult commandResult;
            try
            {
                Log.Information($"user deleted started {userId}");
                commandResult = await _mediator.Send(new DeleteUserCommand() { UserId = userId, UpdatedBy = UserData.UserId }).ConfigureAwait(false);
                Log.Information($"user deleted completed {userId}");
                return Ok(commandResult);
            }
            catch (Exception ex)
            {
                Log.Information(ex, $"Error occurred while deleting user {userId} ");
                commandResult = new CommandResult()
                {
                    Message = Constraints.ErrorResponse
                };
            };
            return InternalServerError(commandResult);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SaveInfo")]
        public async Task<ActionResult<CommandResult>> SaveInfo(UserInfoDto info)
        {
            CommandResult commandResult = new CommandResult();
            try
            {
                commandResult = await _mediator.Send(new UpsertUserInfoCommand() { UserInfo = info }).ConfigureAwait(false);
                if (commandResult.Successful)
                {
                    return Ok(commandResult);
                }
                else
                {
                    Log.Information("Error occurred. Please try again. ");
                    commandResult.Message = "Error occurred. Please try again. ";
                    commandResult.StatusCode = (int)StatusCodesEnum.Error;
                    return Ok(commandResult);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while saving client info");
                commandResult.Message = "Error occurred. Please try again. ";
                commandResult.StatusCode = (int)StatusCodesEnum.Error;
                return InternalServerError(commandResult);
            }
        }

        [HttpGet("Roles")]
        public async Task<ActionResult<CommandResult>> Roles()
        {
            try
            {
                CommandResult cmd = await _mediator.Send(new GetRolesQuery() { }).ConfigureAwait(false);
                return Ok(cmd);
            }
            catch (Exception ex)
            {
                CommandResult cmd = new CommandResult
                {
                    StatusCode = (int)StatusCodesEnum.Error,
                    Message = Constraints.ErrorResponse,
                    Successful = false,
                };
                return cmd;
            }
        }


        [HttpPost("UsersList")]
        public async Task<ActionResult<CommandResult>> UsersList(UserSearchDto searchFilter)
        {
            try
            {
                searchFilter.RoleId = Convert.ToInt32(UserData.RoleId);
                searchFilter.UserId = UserData.UserId;
                CommandResult cmd = await _mediator.Send(new GetUsersListQuery() { filter = searchFilter }).ConfigureAwait(false);
                return Ok(cmd);
            }
            catch (Exception ex)
            {
                CommandResult cmd = new CommandResult
                {
                    StatusCode = (int)StatusCodesEnum.Error,
                    Message = Constraints.ErrorResponse,
                    Successful = false,
                };
                return cmd;
            }
        }
        [AllowAnonymous]
        [HttpGet("SendOTP")]
        public async Task<CommandResult> SendOTP(string userName)
        {
            CommandResult cmdResult;
            try
            {
                Log.Information("Requesting for new OTP");
                cmdResult = await _mediator.Send(new SendOTPCommand() { userName = userName }).ConfigureAwait(false);
                Log.Information("Successfully new OTP sent");
            }
            catch (Exception ex)
            {
                Log.Error($"Something went wrong: {ex}");
                cmdResult = new CommandResult
                {
                    Successful = false,
                    StatusCode = (int)StatusCodesEnum.Error,
                    Message = "Error Occurred try again later ",
                };
            }
            return cmdResult;
        }
    }
}
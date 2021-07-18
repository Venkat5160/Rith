using MediatR;
using Microsoft.EntityFrameworkCore;
using SDAE.Common;
using SDAE.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using SDAE.Api.Application.Account.Models;
using System.Linq;
using SDAE.Api.Application.Infrastructure.Security;

namespace SDAE.Api.Application.Account.Commands
{
    public class SendOTPCommandHandler : IRequestHandler<SendOTPCommand, CommandResult>
    {
        private readonly SDAEDBContext _dataContext;
        public SendOTPCommandHandler(SDAEDBContext SDAEDBContext)
        {
            _dataContext = SDAEDBContext ?? throw new ArgumentNullException(nameof(SDAEDBContext));

        }

        public async Task<CommandResult> Handle(SendOTPCommand request, CancellationToken cancellationToken)
        {

            CommandResult command = new CommandResult();
            var userData = await (from user in _dataContext.AspNetUsers.AsNoTracking()
                                  where user.UserName == request.userName
                                  select (new UserDto()
                                  {
                                      UserId = user.Id,
                                      UserName = user.UserName,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      Email = user.Email,
                                      Contact = user.PhoneNumber,
                                      Role = user.AspNetUserRoles.FirstOrDefault().Role.Name,
                                      RoleId = user.AspNetUserRoles.FirstOrDefault().Role.Id,
                                      Status = user.StatusTypeId == (byte)StatusTypesEnum.Active ? "Active" : "InActive",
                                      StatusTypeId = user.StatusTypeId,
                                  })).FirstOrDefaultAsync().ConfigureAwait(false);

            if (userData != null)
            {
                // Send SMS
                string otpVal = RandomSeriesGenerator.OTPSeries().ToString();
                //SMSQueuesDto item = new SMSQueuesDto
                //{
                //    Smsbody = otpVal + " is your OTP. Please DO NOT share this OTP with anyone to ensure account's security.",
                //    Smsto = unqRes.Mobile,
                //    Smsfrom = _settings.Value.SupportContact
                //};
                //var otpRes = await _iSMSNotifications.SendSMS(item);

                command.Successful = true;
                command.StatusCode = (int)StatusCodesEnum.Ok;
                command.Message = "OTP Sent Successfully";
                command.Result = new { OTP = otpVal, userId = userData.UserId };
            }
            else
            {
                command.Successful = false;
                command.StatusCode = (int)StatusCodesEnum.Error;
                command.Message = "No Data found";
            }
            return command;
        }
    }
}

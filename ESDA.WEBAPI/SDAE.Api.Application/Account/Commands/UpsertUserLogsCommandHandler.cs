using SDAE.Common;
using SDAE.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SDAE.Api.Application.Account.Commands
{
    public class UpsertUserLogsCommandHandler : IRequestHandler<UpsertUserLogsCommand, CommandResult>
    {
        private readonly SDAEDBContext _dataContext;

        public UpsertUserLogsCommandHandler(SDAEDBContext SDAEDBContext)
        {
            _dataContext = SDAEDBContext ?? throw new ArgumentNullException(nameof(SDAEDBContext));
        }

        public async Task<CommandResult> Handle(UpsertUserLogsCommand request, CancellationToken cancellationToken)
        {
            CommandResult commandResult = new CommandResult();
            var user = await _dataContext.UserLogs.FirstOrDefaultAsync(u => u.UserId == request.UserLogsDto.UserId
            && u.SessionId == request.UserLogsDto.SessionId).ConfigureAwait(false);

            if (user == null)
            {
                await _dataContext.UserLogs.AddAsync(GetDBEntity(request)).ConfigureAwait(false);
            }
            else
            {
                user.LogoutTime = Formatter.GetUtcDateTime();
                user.LoginStatusTypeId = (int)LoginStatusTypesEnum.Logout;
                _dataContext.UserLogs.Update(user);
            }
            var result = await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            if (result > 0)
            {
                commandResult.StatusCode = (int)StatusCodesEnum.Ok;
                commandResult.Successful = true;
                commandResult.Result = result;
            }
            else
            {
                commandResult.Result = null;
            }

            return commandResult;
        }
        private UserLogs GetDBEntity(UpsertUserLogsCommand logsCommand)
        {
            return new UserLogs()
            {
                UserName = logsCommand.UserLogsDto.UserName,
                UserId = logsCommand.UserLogsDto.UserId,
                ClientIp = logsCommand.UserLogsDto.ClientIp,
                LogonTime = logsCommand.UserLogsDto.LogonTime,
                LogoutTime = logsCommand.UserLogsDto.LogoutTime,
                SessionId = logsCommand.UserLogsDto.SessionId,
                LoginStatusTypeId = logsCommand.UserLogsDto.LoginStatusTypeId,
                LoginDeviceTypeId = logsCommand.UserLogsDto.LoginDeviceTypeId,
                DeviceToken = logsCommand.UserLogsDto.DeviceToken,
                DeviceModel = logsCommand.UserLogsDto.DeviceModel
            };
        }
    }
}

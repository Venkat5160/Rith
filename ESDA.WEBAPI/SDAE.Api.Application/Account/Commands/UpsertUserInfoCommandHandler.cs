using SDAE.Common;
using SDAE.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SDAE.Api.Application.Account.Commands
{
    public class UpsertUserInfoCommandHandler : IRequestHandler<UpsertUserInfoCommand, CommandResult>
    {
        private readonly SDAEDBContext _dataContext;

        public UpsertUserInfoCommandHandler(SDAEDBContext SDAEDBContext)
        {
            _dataContext = SDAEDBContext ?? throw new ArgumentNullException(nameof(SDAEDBContext));
        }

        public async Task<CommandResult> Handle(UpsertUserInfoCommand request, CancellationToken cancellationToken)
        {
            ClientInfo info = new ClientInfo() { Info = request.UserInfo.Info, AddedDate = Formatter.GetUtcDateTime() };
            await _dataContext.ClientInfo.AddAsync(info).ConfigureAwait(false);
            await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            CommandResult cmdResult = new CommandResult()
            {
                Successful = true,
                StatusCode = (int)StatusCodesEnum.Ok,
                Message = Constraints.AddResponse,
            };

            return cmdResult;
        }

    }
}

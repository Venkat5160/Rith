using SDAE.Api.Application.Account.Models;
using SDAE.Common;
using MediatR;

namespace SDAE.Api.Application.Account.Commands
{
    public class UpsertUserLogsCommand : IRequest<CommandResult>
    {
        public UserLogsDto UserLogsDto { get; set; }
    }
}

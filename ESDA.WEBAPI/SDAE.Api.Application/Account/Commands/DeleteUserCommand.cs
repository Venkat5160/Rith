using SDAE.Common;
using MediatR;

namespace SDAE.Api.Application.Account.Commands
{
    public class DeleteUserCommand : IRequest<CommandResult>
    {
        public string UserId { get; set; }
        public string UpdatedBy { get; set; }

    }
}

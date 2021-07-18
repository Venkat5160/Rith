using SDAE.Common;
using MediatR;

namespace SDAE.Api.Application.Account.Commands
{
    public class UpsertRoleCommand : IRequest<CommandResult>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}

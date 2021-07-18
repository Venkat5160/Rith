using SDAE.Api.Application.Account.Models;
using SDAE.Common;
using MediatR;

namespace SDAE.Api.Application.Account.Commands
{
    public class UpsertUserInfoCommand : IRequest<CommandResult>
    {
        public UserInfoDto UserInfo { get; set; }
    }
}

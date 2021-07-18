using SDAE.Common;
using MediatR;
using System;

namespace SDAE.Api.Application.Account.Queries
{
    public class GetUsersQuery : IRequest<CommandResult>
    {
        public string UserId { get; set; }
    }
}

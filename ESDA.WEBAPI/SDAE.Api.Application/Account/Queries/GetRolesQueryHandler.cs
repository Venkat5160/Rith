using MediatR;
using Microsoft.EntityFrameworkCore;
using SDAE.Common;
using SDAE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDAE.Api.Application.Account.Queries
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, CommandResult>
    {
        private readonly SDAEDBContext _dbContext;
        public GetRolesQueryHandler(SDAEDBContext context)
        {
            _dbContext = context;
        }
        public async Task<CommandResult> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            CommandResult command = new CommandResult();
            var roles = await _dbContext.AspNetRoles.ToListAsync().ConfigureAwait(false);
            if (roles == null)
            {
                command.StatusCode = (int)StatusCodesEnum.Error;
                command.Successful = false;
            }
            else
            {
                command.StatusCode = (int)StatusCodesEnum.Ok;
                command.Successful = true;
                command.Result = new { roles };
            }
            return command;
        }
    }
}

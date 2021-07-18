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
    public class UpsertRoleCommandHandler : IRequestHandler<UpsertRoleCommand, CommandResult>
    {
        private readonly SDAEDBContext _dataContext;

        public UpsertRoleCommandHandler(SDAEDBContext SDAEDBContext)
        {
            _dataContext = SDAEDBContext ?? throw new ArgumentNullException(nameof(SDAEDBContext));
        }

        public async Task<CommandResult> Handle(UpsertRoleCommand request, CancellationToken cancellationToken)
        {
            AspNetUserRoles aspRoles = await _dataContext.AspNetUserRoles.Where(o => o.UserId == request.UserId).FirstOrDefaultAsync().ConfigureAwait(false);
            if (aspRoles == null)
            {
                aspRoles = new AspNetUserRoles { UserId = request.UserId, RoleId = request.RoleId };
            }
            else if (aspRoles != null && aspRoles.RoleId != request.RoleId)
            {
                _dataContext.AspNetUserRoles.Remove(aspRoles);
                _dataContext.SaveChanges();
                aspRoles = new AspNetUserRoles { UserId = request.UserId, RoleId = request.RoleId };
            }
            _dataContext.AspNetUserRoles.Add(aspRoles);
            await _dataContext.SaveChangesAsync().ConfigureAwait(false);

            CommandResult cmdResult = new CommandResult()
            {
                Message = Constraints.UpdateResponse,
            };

            return cmdResult;
        }

    }
}

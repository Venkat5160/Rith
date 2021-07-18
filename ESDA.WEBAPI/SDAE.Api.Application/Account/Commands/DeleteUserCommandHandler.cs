using SDAE.Common;
using SDAE.Data;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SDAE.Api.Application.Account.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, CommandResult>
    {
        private readonly SDAEDBContext _dataContext;

        public DeleteUserCommandHandler(SDAEDBContext sDAEDBContext)
        {
            _dataContext = sDAEDBContext ?? throw new ArgumentNullException(nameof(sDAEDBContext));
        }

        public async Task<CommandResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            CommandResult cmd = new CommandResult();
            var user = _dataContext.AspNetUsers.Where(o => o.Id == request.UserId && o.StatusTypeId == (byte)StatusTypesEnum.Active).FirstOrDefault();
            if (user != null)
            {
                user.StatusTypeId = (int)StatusTypesEnum.InActive;
                user.UpdatedBy = request.UpdatedBy;
                user.UpdatedOn = Formatter.GetUtcDateTime();


                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
                cmd.Message = Constraints.DeleteResponse;
                cmd.StatusCode = 200;
            }
            else
            {
                cmd.Message = Constraints.NotFoundResponse;
                cmd.StatusCode = 404;
            }

            return cmd;
        }
    }
}

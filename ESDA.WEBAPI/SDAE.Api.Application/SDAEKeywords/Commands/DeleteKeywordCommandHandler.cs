using MediatR;
using SDAE.Common;
using SDAE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDAE.Api.Application.SDAEKeywords.Commands
{
	public class DeleteKeywordCommandHandler : IRequestHandler<DeleteKeywordCommand, CommandResult>
    {
        private readonly SDAEDBContext _dataContext;

        public DeleteKeywordCommandHandler(SDAEDBContext sDAEDBContext)
        {
            _dataContext = sDAEDBContext ?? throw new ArgumentNullException(nameof(sDAEDBContext));
        }

        public async Task<CommandResult> Handle(DeleteKeywordCommand request, CancellationToken cancellationToken)
        {
            CommandResult cmd = new CommandResult();
            var user = _dataContext.Sdaekeywords.Where(o => o.SdaekeywordId == request.SdaekeywordId && o.StatusTypeId == (byte)StatusTypesEnum.Active).FirstOrDefault();
            if (user != null)
            {
                user.StatusTypeId = (int)StatusTypesEnum.InActive;
                user.ModifiedBy = request.UpdatedBy;
                user.ModifiedDate = Formatter.GetUtcDateTime();

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

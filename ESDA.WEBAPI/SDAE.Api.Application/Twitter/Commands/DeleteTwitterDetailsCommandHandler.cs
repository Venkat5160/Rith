using MediatR;
using SDAE.Common;
using SDAE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDAE.Api.Application.Twitter.Commands
{
	public class DeleteTwitterDetailsCommandHandler : IRequestHandler<DeleteTwitterDetailsCommand, CommandResult>
	{
		private readonly SDAEDBContext _dataContext;

		public DeleteTwitterDetailsCommandHandler(SDAEDBContext sDAEDBContext)
		{
			_dataContext = sDAEDBContext ?? throw new ArgumentNullException(nameof(sDAEDBContext));
		}

		public async Task<CommandResult> Handle(DeleteTwitterDetailsCommand request, CancellationToken cancellationToken)
		{
			CommandResult cmd = new CommandResult();
			var user = _dataContext.TwitterDetails.Where(o => o.TwitterId == request.TwitterId && o.StatusTypeId == (byte)StatusTypesEnum.Active).FirstOrDefault();
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

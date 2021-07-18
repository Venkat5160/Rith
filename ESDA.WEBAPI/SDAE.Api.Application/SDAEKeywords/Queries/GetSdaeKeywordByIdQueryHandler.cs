using SDAE.Common;
using SDAE.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SDAE.Api.Application.SDAEKeywords.Models;

namespace SDAE.Api.Application.SDAEKeywords.Queries
{
	public class GetSdaeKeywordByIdQueryHandler : IRequestHandler<GetSdaeKeywordByIdQuery, CommandResult>
	{
		private readonly SDAEDBContext _dbContext;
		public GetSdaeKeywordByIdQueryHandler(SDAEDBContext context)
		{
			_dbContext = context;
		}

		public async Task<CommandResult> Handle(GetSdaeKeywordByIdQuery request, CancellationToken cancellationToken)
		{
			CommandResult command = new CommandResult();
			var keyword = await _dbContext.Sdaekeywords.Where(o => o.SdaekeywordId == request.SdaekeywordId).FirstOrDefaultAsync().ConfigureAwait(false);

			if (keyword != null)
			{
				var keywordDto = new SdaekeywordsDto() { SdaekeywordId = keyword.SdaekeywordId, Keyword = keyword.Name };
				command.Result = keywordDto;
				command.Successful = true;
				command.StatusCode = (int)StatusCodesEnum.Ok;
			}
			else
				command.Result = null;
			return command;
		}
	}
}

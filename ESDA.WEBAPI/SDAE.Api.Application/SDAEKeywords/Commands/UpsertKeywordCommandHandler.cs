using MediatR;
using SDAE.Common;
using SDAE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace SDAE.Api.Application.SDAEKeywords.Commands
{
	public class UpsertKeywordCommandHandler : IRequestHandler<UpsertKeywordCommand, CommandResult>
	{

		private readonly SDAEDBContext _askanexpertContext;
		public UpsertKeywordCommandHandler(SDAEDBContext askanexpertContext)
		{
			_askanexpertContext = askanexpertContext ?? throw new ArgumentNullException(nameof(askanexpertContext));
		}

		public async Task<CommandResult> Handle(UpsertKeywordCommand request, CancellationToken cancellationToken)
		{
			int result = 0;
			CommandResult cmdRes = new CommandResult();

			List<Sdaekeywords> listData = _askanexpertContext.Sdaekeywords.Where(o => o.Name == request.SdaeKeyword.Keyword && o.StatusTypeId == (byte)StatusTypesEnum.Active).ToList();
			if (listData.Count == 0)
			{
				var sdaekeyword = await _askanexpertContext.Sdaekeywords.Where(prd => (prd.SdaekeywordId == request.SdaeKeyword.SdaekeywordId && prd.StatusTypeId == (byte)StatusTypesEnum.Active)).FirstOrDefaultAsync().ConfigureAwait(false);
				if (sdaekeyword == null)
				{
					sdaekeyword = new Sdaekeywords();

					sdaekeyword.Name = request.SdaeKeyword.Keyword;
					sdaekeyword.CreatedBy = request.SdaeKeyword.CreatedBy;
					sdaekeyword.CreatedDate = Formatter.GetUtcDateTime();
					sdaekeyword.StatusTypeId = (byte)StatusTypesEnum.Active;
					await _askanexpertContext.Sdaekeywords.AddAsync(sdaekeyword).ConfigureAwait(false);
				}
				else
				{
					sdaekeyword.ModifiedBy = request.SdaeKeyword.ModifiedBy;
					sdaekeyword.ModifiedDate = Formatter.GetUtcDateTime();
					sdaekeyword.Name = request.SdaeKeyword.Keyword;

				}
				result = await _askanexpertContext.SaveChangesAsync().ConfigureAwait(false);
				if (result > 0)
				{
					cmdRes.Successful = true;
					cmdRes.StatusCode = (int)StatusCodesEnum.Ok;
					cmdRes.Message = request.SdaeKeyword.SdaekeywordId > 0 ? Constraints.UpdateResponse : Constraints.AddResponse;
				}
				else
				{
					cmdRes.Successful = false;
					cmdRes.StatusCode = (int)StatusCodesEnum.Error;
					cmdRes.Message = Constraints.ErrorResponse;
				}
			}
            else
            {
				cmdRes.Successful = false;
				cmdRes.StatusCode = (int)StatusCodesEnum.Error;
				cmdRes.Message = "Keyword already exist";
			}

			return cmdRes;
		}
	}
}

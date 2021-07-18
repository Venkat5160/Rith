using MediatR;
using SDAE.Api.Application.SDAEKeywords.Models;
using SDAE.Common;
using SDAE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace SDAE.Api.Application.SDAEKeywords.Queries
{

	public class GetSdaeKeywordsMinDetailsQueryHandler : IRequestHandler<GetSdaeKeywordsMinDetailsQuery, CommandResult>
	{
		private readonly SDAEDBContext _dataContext;

		public GetSdaeKeywordsMinDetailsQueryHandler(SDAEDBContext sDAEDBContext)
		{
			_dataContext = sDAEDBContext ?? throw new ArgumentNullException(nameof(sDAEDBContext));
		}

		public async Task<CommandResult> Handle(GetSdaeKeywordsMinDetailsQuery request, CancellationToken cancellationToken)
		{
			CommandResult cmdRes = new CommandResult();
			SdaekeywordsMinSearchDto srchFilter = request.filter;

			List<SdaeMinkeywordsDto> sdaekeywords = new List<SdaeMinkeywordsDto>();
			var predicate = PredicateKeywords(request.filter);

			var keyWords = (from kw in _dataContext.Sdaekeywords.Where(predicate)
							select new SdaeMinkeywordsDto()
							{
								SdaekeywordId = kw.SdaekeywordId,
								Keyword = kw.Name,
								CreatedDate = kw.CreatedDate,
								ModifiedDate = kw.ModifiedDate
							});

			keyWords = keyWords.OrderBy(m => m.SdaekeywordId);
			int totalRecords = await keyWords.CountAsync().ConfigureAwait(false);

			if (request.filter.PageSize > 0 && request.filter.PageNumber > 0 && totalRecords > request.filter.PageSize)
			{
				sdaekeywords = await keyWords.Skip((request.filter.PageNumber - 1) * request.filter.PageSize).Take(request.filter.PageSize).ToListAsync().ConfigureAwait(false);
			}
			else
			{
				sdaekeywords = await keyWords.ToListAsync().ConfigureAwait(false);
			}

			if (sdaekeywords == null || sdaekeywords.Count() == 0)
			{
				cmdRes.StatusCode = (int)StatusCodesEnum.Error;
				cmdRes.Successful = false;
			}
			else
			{
				cmdRes.StatusCode = (int)StatusCodesEnum.Ok;
				cmdRes.Successful = true;
				cmdRes.Result = new { totalRecords, sdaekeywords };
			}

			return cmdRes;
		}

		public Expression<Func<Sdaekeywords, bool>> PredicateKeywords(SdaekeywordsMinSearchDto srchFilter)
		{
			var predicate = PredicateBuilder.New<Sdaekeywords>();
			predicate = predicate.And(m => m.StatusTypeId == (int)StatusTypesEnum.Active);
			if (!string.IsNullOrEmpty(srchFilter.Keyword))
			{
				predicate = predicate.And(m => m.Name.Contains(srchFilter.Keyword));
			}			
			return predicate;
		}
	}
}

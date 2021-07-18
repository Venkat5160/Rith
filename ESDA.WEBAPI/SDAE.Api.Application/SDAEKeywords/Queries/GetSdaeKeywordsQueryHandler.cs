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

	public class GetSdaeKeywordsQueryHandler : IRequestHandler<GetSdaeKeywordsQuery, CommandResult>
	{
		private readonly SDAEDBContext _dataContext;

		public GetSdaeKeywordsQueryHandler(SDAEDBContext sDAEDBContext)
		{
			_dataContext = sDAEDBContext ?? throw new ArgumentNullException(nameof(sDAEDBContext));
		}

		public async Task<CommandResult> Handle(GetSdaeKeywordsQuery request, CancellationToken cancellationToken)
		{
			CommandResult cmdRes = new CommandResult();
			SdaekeywordsSearchDto srchFilter = request.filter;

			List<SdaekeywordsDto> sdaekeywords = new List<SdaekeywordsDto>();
			var predicate = PredicateKeywords(request.filter);

			var keyWords = (from kw in _dataContext.Sdaekeywords.Where(predicate)
							join cd in _dataContext.AspNetUsers on kw.CreatedBy equals cd.Id
							join md in _dataContext.AspNetUsers on kw.ModifiedBy equals md.Id into ud
							from ub in ud.DefaultIfEmpty()						
							select new SdaekeywordsDto()
							{
								SdaekeywordId = kw.SdaekeywordId,
                                Keyword = kw.Name,
                                CreatedBy = (cd.FirstName + " " + cd.LastName),
                                CreatedDate = kw.CreatedDate,
                                ModifiedDate = kw.ModifiedDate,
                                ModifiedBy = ub != null ? (ub.FirstName + " " + ub.LastName) : ""
                            });

			if (!string.IsNullOrEmpty(srchFilter.AddedBy))
				keyWords = keyWords.Where(o => o.CreatedBy.ToLower().Contains(srchFilter.AddedBy.ToLower()));

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
				int slno = (request.filter.PageNumber - 1) * request.filter.PageSize;
				foreach (var keyword in sdaekeywords)
				{
					++slno;
					keyword.SlNo = slno;
				}
				cmdRes.StatusCode = (int)StatusCodesEnum.Ok;
				cmdRes.Successful = true;
				cmdRes.Result = new { totalRecords, sdaekeywords };
			}

			return cmdRes;
		}

		public Expression<Func<Sdaekeywords, bool>> PredicateKeywords(SdaekeywordsSearchDto srchFilter)
		{
			var predicate = PredicateBuilder.New<Sdaekeywords>();
			predicate = predicate.And(m => m.StatusTypeId == (int)StatusTypesEnum.Active);
			if (!string.IsNullOrEmpty(srchFilter.Keyword))
			{
				predicate = predicate.And(m => m.Name.Contains(srchFilter.Keyword));
			}
			//if (!string.IsNullOrEmpty(srchFilter.AddedBy))
			//	predicate = predicate.And(m => m.CreatedBy.Contains(srchFilter.AddedBy));
			return predicate;
		}
	}
}

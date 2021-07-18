using MediatR;
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
using SDAE.Api.Application.Twitter.Models;

namespace SDAE.Api.Application.Twitter.Queries
{

	public class GetTwitterDetailsQueryHandler : IRequestHandler<GetTwitterDetailsQuery, CommandResult>
	{
		private readonly SDAEDBContext _dataContext;

		public GetTwitterDetailsQueryHandler(SDAEDBContext sDAEDBContext)
		{
			_dataContext = sDAEDBContext ?? throw new ArgumentNullException(nameof(sDAEDBContext));
		}

		public async Task<CommandResult> Handle(GetTwitterDetailsQuery request, CancellationToken cancellationToken)
		{
			CommandResult cmdRes = new CommandResult();


			var predicate = PredicateKeywords(request.filter);

			var twitterDetails = await (from td in _dataContext.TwitterDetails.Where(predicate)
										join cd in _dataContext.AspNetUsers on td.CreatedBy equals cd.Id
										join md in _dataContext.AspNetUsers on td.ModifiedBy equals md.Id into ud
										from ub in ud.DefaultIfEmpty()
										select new TwitterDetailsDto()
										{
											TwitterId = td.TwitterId,
											Username = td.Username,
											Password = td.Password,
											ClientId = td.ClientId,
											SecretKey = td.SecretKey,
											Url = td.Url,
											CreatedBy = (cd.FirstName + " " + cd.LastName),
											CreatedDate = td.CreatedDate,
											ModifiedDate = td.ModifiedDate,
											ModifiedBy = ub != null ? (ub.FirstName + " " + ub.LastName) : ""
										}).FirstOrDefaultAsync().ConfigureAwait(false);

			var totalCount = 1;
			if (twitterDetails == null)
			{
				cmdRes.StatusCode = (int)StatusCodesEnum.Error;
				cmdRes.Successful = false;
			}
			else
			{

				cmdRes.StatusCode = (int)StatusCodesEnum.Ok;
				cmdRes.Successful = true;
				cmdRes.Result = new { totalCount, twitterDetails };
			}

			return cmdRes;
		}

		public Expression<Func<TwitterDetails, bool>> PredicateKeywords(TwitterDetailsSearchDto srchFilter)
		{
			var predicate = PredicateBuilder.New<TwitterDetails>();
			predicate = predicate.And(m => m.StatusTypeId == (int)StatusTypesEnum.Active);
			if (!string.IsNullOrEmpty(srchFilter.AddedBy))
				predicate = predicate.And(m => m.CreatedBy.Contains(srchFilter.AddedBy));
			return predicate;
		}
	}
}

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


namespace SDAE.Api.Application.Twitter.Commands
{
	public class UpsertTwitterDetailsCommandHandler : IRequestHandler<UpsertTwitterDetailsCommand, CommandResult>
	{

		private readonly SDAEDBContext _sDAEDBContext;
		public UpsertTwitterDetailsCommandHandler(SDAEDBContext askanexpertContext)
		{
			_sDAEDBContext = askanexpertContext ?? throw new ArgumentNullException(nameof(askanexpertContext));
		}

		public async Task<CommandResult> Handle(UpsertTwitterDetailsCommand request, CancellationToken cancellationToken)
		{
			int result = 0;
			CommandResult cmdRes = new CommandResult();
			var twitterDetails = await _sDAEDBContext.TwitterDetails.Where(prd => (prd.TwitterId == request.TwitterDetailsDto.TwitterId && prd.StatusTypeId == (byte)StatusTypesEnum.Active)).FirstOrDefaultAsync().ConfigureAwait(false);
			if (twitterDetails == null)
			{
				twitterDetails = new TwitterDetails();
				twitterDetails.Username = request.TwitterDetailsDto.Username;
				twitterDetails.Password = request.TwitterDetailsDto.Password;
				twitterDetails.ClientId = request.TwitterDetailsDto.ClientId;
				twitterDetails.SecretKey = request.TwitterDetailsDto.SecretKey;
				twitterDetails.Url = request.TwitterDetailsDto.Url;
				twitterDetails.CreatedBy = request.TwitterDetailsDto.CreatedBy;
				twitterDetails.CreatedDate = Formatter.GetUtcDateTime();
				twitterDetails.StatusTypeId = (byte)StatusTypesEnum.Active;

				await _sDAEDBContext.TwitterDetails.AddAsync(twitterDetails).ConfigureAwait(false);
			}
			else
			{
				twitterDetails.ModifiedBy = request.TwitterDetailsDto.ModifiedBy;
				twitterDetails.ModifiedDate = Formatter.GetUtcDateTime();
				twitterDetails.Username = request.TwitterDetailsDto.Username;
				twitterDetails.Password = request.TwitterDetailsDto.Password;
				twitterDetails.ClientId = request.TwitterDetailsDto.ClientId;
				twitterDetails.SecretKey = request.TwitterDetailsDto.SecretKey;
				twitterDetails.Url = request.TwitterDetailsDto.Url;

			}
			result = await _sDAEDBContext.SaveChangesAsync().ConfigureAwait(false);
			if (result > 0)
			{
				cmdRes.Successful = true;
				cmdRes.StatusCode = (int)StatusCodesEnum.Ok;
				cmdRes.Message = request.TwitterDetailsDto.TwitterId > 0 ? Constraints.UpdateResponse : Constraints.AddResponse;
			}
			else
			{
				cmdRes.Successful = false;
				cmdRes.StatusCode = (int)StatusCodesEnum.Error;
				cmdRes.Message = Constraints.ErrorResponse;
			}

			return cmdRes;
		}
	}
}

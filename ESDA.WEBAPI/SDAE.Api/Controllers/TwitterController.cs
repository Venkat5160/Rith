using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechRAQ.Common.LoggingService.Serilog.Interfaces.Service;
using SDAE.Api.Application.SDAEKeywords.Commands;
using SDAE.Data.Model.OptionSettings;
using SDAE.Common;
using SDAE.Api.Application.SDAEKeywords.Models;
using SDAE.Api.Application.SDAEKeywords.Queries;
using Serilog;
using SDAE.Api.Application.Twitter.Queries;
using SDAE.Api.Application.Twitter.Models;
using SDAE.Api.Application.Twitter.Commands;

namespace SDAE.Api.Controllers
{
    public class TwitterController : BaseApiController
	{
		private readonly AppSettings _appSettings;
		public IWebHostEnvironment _hostingEnvironment { get; }
		public TwitterController(IMediator mediator, ILoggingService loggingService, IWebHostEnvironment hostingEnvironment, IOptions<AppSettings> appSettings) : base(mediator, loggingService)
		{
			_hostingEnvironment = hostingEnvironment;
			_appSettings = appSettings.Value;
		}

		[HttpGet("GetTwitterDetails")]
		public async Task<ActionResult<CommandResult>> GetTwitterDetails()
		{
			try
			{
				TwitterDetailsSearchDto searchFilter = new TwitterDetailsSearchDto() { AddedBy = UserData.UserId };
				CommandResult cmd = await _mediator.Send(new GetTwitterDetailsQuery() { filter = searchFilter }).ConfigureAwait(false);
				return Ok(cmd);
			}
			catch (Exception ex)
			{
				CommandResult cmd = new CommandResult
				{
					StatusCode = (int)StatusCodesEnum.Error,
					Message = Constraints.ErrorResponse,
					Successful = false,
				};
				return cmd;
			}
		}

		[HttpPost("SaveTwitterDetails")]
		public async Task<ActionResult<CommandResult>> SaveTwitterDetails([FromBody] TwitterDetailsDto twitterDetailsDto)
		{
			try
			{
				if (twitterDetailsDto.TwitterId == 0)
					twitterDetailsDto.CreatedBy = UserData.UserId;
				else
					twitterDetailsDto.ModifiedBy = UserData.UserId;
				CommandResult cmd = await _mediator.Send(new UpsertTwitterDetailsCommand() { TwitterDetailsDto = twitterDetailsDto }).ConfigureAwait(false);
				return Ok(cmd);
			}
			catch (Exception ex)
			{
				CommandResult cmd = new CommandResult
				{
					StatusCode = (int)StatusCodesEnum.Error,
					Message = Constraints.ErrorResponse,
					Successful = false,
				};
				return cmd;
			}
		}

		[HttpDelete]
		[Route("DeleteTwitterDetails/{twitterId}")]
		public async Task<ActionResult<CommandResult>> DeleteTwitterDetails(int twitterId)
		{
			CommandResult commandResult;
			try
			{
				Log.Information($"TwitterDetails deleted started {twitterId}");
				commandResult = await _mediator.Send(new DeleteTwitterDetailsCommand() { TwitterId = twitterId, UpdatedBy = UserData.UserId }).ConfigureAwait(false);
				Log.Information($"TwitterDetails deleted completed {twitterId}");
				return Ok(commandResult);
			}
			catch (Exception ex)
			{
				Log.Information(ex, $"Error occured while deleting TwitterDetails {twitterId} ");
				commandResult = new CommandResult()
				{
					Message = Constraints.ErrorResponse
				};
			};
			return InternalServerError(commandResult);
		}
	}
}
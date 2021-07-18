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

namespace SDAE.Api.Controllers
{
	public class SDAEKeywordsController : BaseApiController
	{
		private readonly AppSettings _appSettings;
		public IWebHostEnvironment _hostingEnvironment { get; }
		public SDAEKeywordsController(IMediator mediator, ILoggingService loggingService, IWebHostEnvironment hostingEnvironment, IOptions<AppSettings> appSettings) : base(mediator, loggingService)
		{
			_hostingEnvironment = hostingEnvironment;
			_appSettings = appSettings.Value;
		}

		[HttpPost("KeywordsList")]
		public async Task<ActionResult<CommandResult>> KeywordsList(SdaekeywordsSearchDto searchFilter)
		{
			try
			{
				CommandResult cmd = await _mediator.Send(new GetSdaeKeywordsQuery() { filter = searchFilter }).ConfigureAwait(false);
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

		[HttpPost("GetKeywords")]
		[AllowAnonymous]
		public async Task<ActionResult<CommandResult>> KeywordsListForPublic(SdaekeywordsMinSearchDto searchFilter)
		{
			try
			{
				CommandResult cmd = await _mediator.Send(new GetSdaeKeywordsMinDetailsQuery() { filter = searchFilter }).ConfigureAwait(false);
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

		[HttpPost("SaveKeywords")]
		public async Task<ActionResult<CommandResult>> SaveKeywords([FromBody] SdaekeywordsDto sdaekeywordsDto)
		{
			try
			{
				if (sdaekeywordsDto.SdaekeywordId == 0)
					sdaekeywordsDto.CreatedBy = UserData.UserId;
				else
					sdaekeywordsDto.ModifiedBy = UserData.UserId;


				CommandResult cmd = await _mediator.Send(new UpsertKeywordCommand() { SdaeKeyword = sdaekeywordsDto }).ConfigureAwait(false);
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


		[HttpGet("GetKeywordsById")]
		public async Task<ActionResult<CommandResult>> GetKeyworssById(int keywordId)
		{
			try
			{
				CommandResult cmd = await _mediator.Send(new GetSdaeKeywordByIdQuery() { SdaekeywordId = keywordId }).ConfigureAwait(false);
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
		[Route("DeleteKeyword/{keywordId}")]
		public async Task<ActionResult<CommandResult>> DeleteKeyword(int keywordId)
		{
			CommandResult commandResult;
			try
			{
				Log.Information($"Keyword deleted started {keywordId}");
				commandResult = await _mediator.Send(new DeleteKeywordCommand() { SdaekeywordId = keywordId, UpdatedBy = UserData.UserId }).ConfigureAwait(false);
				Log.Information($"Keyword deleted completed {keywordId}");

				return Ok(commandResult);
			}
			catch (Exception ex)
			{
				Log.Information(ex, $"Error occured while deleting keyword {keywordId} ");
				commandResult = new CommandResult()
				{
					Message = Constraints.ErrorResponse
				};
			};
			return InternalServerError(commandResult);
		}
	}
}
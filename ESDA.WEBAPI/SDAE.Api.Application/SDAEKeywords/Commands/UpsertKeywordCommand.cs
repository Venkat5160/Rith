using MediatR;
using SDAE.Api.Application.SDAEKeywords.Models;
using SDAE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.SDAEKeywords.Commands
{
	public class UpsertKeywordCommand : IRequest<CommandResult>
	{
		public SdaekeywordsDto SdaeKeyword { get; set; }
		public string UserId { get; set; }
	}
}

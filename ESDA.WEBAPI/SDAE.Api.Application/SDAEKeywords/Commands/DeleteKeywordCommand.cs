using MediatR;
using SDAE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.SDAEKeywords.Commands
{
	public class DeleteKeywordCommand : IRequest<CommandResult>
	{
		public int SdaekeywordId { get; set; }
		public string UpdatedBy { get; set; }

	}
}

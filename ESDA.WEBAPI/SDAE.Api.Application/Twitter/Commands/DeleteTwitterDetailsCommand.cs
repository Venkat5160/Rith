using MediatR;
using SDAE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.Twitter.Commands
{
	public class DeleteTwitterDetailsCommand : IRequest<CommandResult>
	{
		public int TwitterId { get; set; }
		public string UpdatedBy { get; set; }

	}
}

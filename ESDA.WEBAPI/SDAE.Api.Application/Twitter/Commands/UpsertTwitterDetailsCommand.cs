using MediatR;
using SDAE.Api.Application.Twitter.Models;
using SDAE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.Twitter.Commands
{
	public class UpsertTwitterDetailsCommand : IRequest<CommandResult>
	{
		public TwitterDetailsDto TwitterDetailsDto { get; set; }
		public string UserId { get; set; }
	}
}

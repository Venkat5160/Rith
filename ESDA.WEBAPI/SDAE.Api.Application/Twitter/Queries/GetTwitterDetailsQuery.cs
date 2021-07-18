using MediatR;
using SDAE.Api.Application.Twitter.Models;
using SDAE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.Twitter.Queries
{
	public class GetTwitterDetailsQuery : IRequest<CommandResult>
    {
        public TwitterDetailsSearchDto filter { get; set; }
    }
}

using MediatR;
using SDAE.Api.Application.Account.Models;
using SDAE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.Account.Queries
{
	public class GetUsersListQuery : IRequest<CommandResult>
	{
		public UserSearchDto filter { get; set; }
	}	
}

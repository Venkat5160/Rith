using SDAE.Api.Application.Account.Models;
using SDAE.Common;
using SDAE.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SDAE.Api.Application.Account.Queries
{
	public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, CommandResult>
	{
		private readonly SDAEDBContext _dbContext;
		public GetUsersQueryHandler(SDAEDBContext context)
		{
			_dbContext = context;
		}
		public async Task<CommandResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
		{
			CommandResult command = new CommandResult();
			var userData = await (from user in _dbContext.AspNetUsers.AsNoTracking()
								 where user.Id == request.UserId
								 select (new UserDto()
								 {
									 UserId = user.Id,
									 UserName = user.UserName,
									 FirstName = user.FirstName,
									 LastName = user.LastName,
									 Email = user.Email,
									 Contact = user.PhoneNumber,
									 Role = user.AspNetUserRoles.FirstOrDefault().Role.Name,
									 RoleId = user.AspNetUserRoles.FirstOrDefault().Role.Id,
									 Status = user.StatusTypeId == (byte)StatusTypesEnum.Active ? "Active" : "InActive",
									 StatusTypeId = user.StatusTypeId,
								 })).FirstOrDefaultAsync().ConfigureAwait(false);
			if (userData != null)
			{
				command.Result = userData;
				command.Successful = true;
				command.StatusCode = (int)StatusCodesEnum.Ok;
			}
			else
				command.Result = null;
			return command;
		}
	}
}

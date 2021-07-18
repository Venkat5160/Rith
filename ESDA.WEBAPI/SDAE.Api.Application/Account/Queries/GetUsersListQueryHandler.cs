using MediatR;
using SDAE.Api.Application.Account.Models;
using SDAE.Common;
using SDAE.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace SDAE.Api.Application.Account.Queries
{
    public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, CommandResult>
    {
        private readonly SDAEDBContext _dbContext;
        public GetUsersListQueryHandler(SDAEDBContext context)
        {
            _dbContext = context;
        }
        public async Task<CommandResult> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            CommandResult command = new CommandResult();
            var predicate = PredicateUsers(request.filter);
            var lstUsers = from user in _dbContext.AspNetUsers.Where(predicate)
                           join cd in _dbContext.AspNetUsers on user.CreatedBy equals cd.Id
                           join md in _dbContext.AspNetUsers on user.UpdatedBy equals md.Id into ud
                           from ub in ud.DefaultIfEmpty()
                           select new UserDto()
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
                               CreatedBy = (cd.FirstName + " " + cd.LastName),
                               CreatedDate = user.CreatedOn,
                               ModifiedDate = user.UpdatedOn,
                               ModifiedBy = ub != null ? (ub.FirstName + " " + ub.LastName) : ""
                           };
            lstUsers = lstUsers.OrderBy(m => m.FirstName);
            int totalRecords = await lstUsers.CountAsync().ConfigureAwait(false);

            if (request.filter.PageSize > 0 && request.filter.PageNumber > 0 && totalRecords > request.filter.PageSize)
            {
                lstUsers = lstUsers.Skip((request.filter.PageNumber - 1) * request.filter.PageSize).Take(request.filter.PageSize);
            }
            if (lstUsers == null)
            {
                command.StatusCode = (int)StatusCodesEnum.Error;
                command.Successful = false;
            }
            else
            {
                command.StatusCode = (int)StatusCodesEnum.Ok;
                command.Successful = true;
                command.Result = new { totalRecords, lstUsers };
            }
            return command;
        }

        public Expression<Func<AspNetUsers, bool>> PredicateUsers(UserSearchDto srchFilter)
        {
            var predicate = PredicateBuilder.New<AspNetUsers>();
            predicate = predicate.And(m => m.StatusTypeId == (int)StatusTypesEnum.Active);
            if (!string.IsNullOrEmpty(srchFilter.Name))
            {
                predicate = predicate.And(m => (m.FirstName + " " + m.LastName).Contains(srchFilter.Name));
            }
            if (!string.IsNullOrEmpty(srchFilter.Email))
                predicate = predicate.And(m => m.Email.Contains(srchFilter.Email));
            if (srchFilter.RoleId == (int)RolesEnum.Executive)
                predicate = predicate.And(m => m.Id == srchFilter.UserId);
            return predicate;
        }
    }
}

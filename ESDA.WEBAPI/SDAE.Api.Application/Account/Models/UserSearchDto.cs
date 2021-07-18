using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.Account.Models
{
	public class UserSearchDto
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
		public string UserId { get; set; }
		public int RoleId { get; set; }

	}
}

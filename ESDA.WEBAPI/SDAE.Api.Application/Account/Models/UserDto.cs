using System;

namespace SDAE.Api.Application.Account.Models
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; }
        public string CallBackUIUrl { get; set; }
        public string Status { get; set; }
        public byte? StatusTypeId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}

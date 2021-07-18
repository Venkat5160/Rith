using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.Twitter.Models
{
	public class TwitterDetailsDto
	{
        public int TwitterId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public byte StatusTypeId { get; set; }

    }
}

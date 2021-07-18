using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.SDAEKeywords.Models
{
	public class SdaekeywordsDto
	{
		public int SlNo { get; set; }
		public int SdaekeywordId { get; set; }
		public string Keyword { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }
		public byte StatusTypeId { get; set; }

	}

	public class SdaeMinkeywordsDto
	{
		public int SdaekeywordId { get; set; }
		public string Keyword { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }

	}
}

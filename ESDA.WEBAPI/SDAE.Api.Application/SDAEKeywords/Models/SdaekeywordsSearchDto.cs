using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.SDAEKeywords.Models
{
	public class SdaekeywordsSearchDto
	{
		public SdaekeywordsSearchDto()
		{
			this.PageSize = 10;
			this.PageNumber = 1;
		}
		public string Keyword { get; set; }
		public string AddedBy { get; set; }
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}

	public class SdaekeywordsMinSearchDto
	{
		public SdaekeywordsMinSearchDto()
		{
			this.PageSize = 10;
			this.PageNumber = 1;
		}
		public string Keyword { get; set; }		
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}
}

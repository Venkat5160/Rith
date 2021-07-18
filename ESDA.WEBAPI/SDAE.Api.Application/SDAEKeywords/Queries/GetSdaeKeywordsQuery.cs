using MediatR;
using SDAE.Api.Application.SDAEKeywords.Models;
using SDAE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.SDAEKeywords.Queries
{
	public class GetSdaeKeywordsQuery : IRequest<CommandResult>
    {
        public SdaekeywordsSearchDto filter { get; set; }
    }
}

using MediatR;
using SDAE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.SDAEKeywords.Queries
{
	public class GetSdaeKeywordByIdQuery : IRequest<CommandResult>
    {
        public int SdaekeywordId { get; set; }
    }
}

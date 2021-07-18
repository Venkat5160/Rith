using MediatR;
using SDAE.Api.Application.SDAEKeywords.Models;
using SDAE.Common;
using SDAE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;
namespace SDAE.Api.Application.SDAEKeywords.Queries
{
	public class GetSdaeKeywordsMinDetailsQuery : IRequest<CommandResult>
    {
        public SdaekeywordsMinSearchDto filter { get; set; }
    }
}

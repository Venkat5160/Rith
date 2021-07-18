using MediatR;
using SDAE.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.Account.Commands
{
    public class SendOTPCommand :IRequest<CommandResult>
    {
        public string userName { get; set; }
    }
}

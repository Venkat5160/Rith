using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Api.Application.Account.Models
{
    public class UserLogsDto
    {
        public long UserLogId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string ClientIp { get; set; }
        public DateTime LogonTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public Guid SessionId { get; set; }
        public int? LoginStatusTypeId { get; set; }
        public short? LoginDeviceTypeId { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceModel { get; set; }
    }
}

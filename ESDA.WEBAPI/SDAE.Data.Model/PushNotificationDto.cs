using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Data.Model
{
    public class PushNotificationDto
    {
        public string IOSAppId { get; set; }
        public string DroidAppId { get; set; }
        public string DeviceToken { get; set; }
        public string Message { set; get; }
        public string Title { set; get; }
        public string TransactionId { get; set; }
        public string Token { get; set; }

        public int IOSPort { get; set; }
        public string IOSHost { get; set; }
        public string IOSCertificatePath { get; set; }
        public string IOSPassword { get; set; }
    }
}

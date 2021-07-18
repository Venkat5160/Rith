using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Data.Model
{
    public class PushNotificationResponse
    {
        public string multicast_id { get; set; }
        public string success { get; set; }
        public string failure { get; set; }
        public string canonical_ids { get; set; }
        public dynamic results { get; set; }

    }
}

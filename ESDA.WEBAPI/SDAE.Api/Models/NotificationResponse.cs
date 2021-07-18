using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDAE.Api.Models
{
    public class NotificationResponse
    {
        public string Token { get; set; }
        public string RoomName { get; set; }
        public string RoomSid { get; set; }
        public string UserId { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
    }
}

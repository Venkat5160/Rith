using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Data.Model.Room
{
    public class UpdateCustCallDto
    {
        public string TransactionId { get; set; }
        public int CallStatusTypeId { get; set; }
    }

    public class CustomerFeedbackDto
    {
        public string TransactionId { get; set; }
        public int CallStatusTypeId { get; set; }
        public int Rating { get; set; }
        public string Notes { get; set; }
    }

    public class CloseRoomDto
    {
        public string TransactionId { get; set; }
        public int VendorTypeId { get; set; }
    }

    public class AgentResponse
    {
        public string UserId { get; set; }
        public long CustomerCallId { get; set; }
        public int BrandId { get; set; }
    }
}

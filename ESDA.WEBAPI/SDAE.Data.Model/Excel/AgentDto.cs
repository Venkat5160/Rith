using System;
using System.Collections.Generic;
using System.Text;

namespace SDAE.Data.Model.Excel
{
    public class AgentDto
    {
        public int AgentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public int TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }
        public string Status { get; set; }
        public string Retailer { get; set; }
    }
}

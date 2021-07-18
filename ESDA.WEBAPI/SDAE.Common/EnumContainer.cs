using System;

namespace SDAE.Common
{
    public enum StatusTypesEnum
    {
        Active = 1,
        InActive = 2,
        Delete = 3
    }

    public enum StatusCodesEnum
    {
        Ok = 200,
        Conflict = 409,
        Error = 500,
    }

    public enum RolesEnum
    {
        SuperAdmin = 1000,
        Admin = 2000,
        ProjectLead = 2001,
        TeamLead = 2002,
        Executive = 2003,
    }
    public enum PowerBiReportIdsEnum
    {
        DashboardReportId = 1,
        AllStatesAnalysisReportId = 2,
        SwingStatesAnalysisReportId = 3,
        DayWiseSentimentReportId = 4,
        PredictReportId = 5

    }
    public enum LoginStatusTypesEnum
    {
        SuccessfulLogin = 1,
        InvalidCredentials = 2,
        InvalidIp = 3,
        Logout = 4
    }

    public enum CallStatusTypesEnum
    {
        CallInitiated = 1,
        CustomerConnected = 2,
        NotificationSentToAgent = 3,
        AgentConnected = 4,
        CallCompleted = 5,
        AgentDeclined = 6,
        AgentNotAnswered = 7,
        AgentNotAvailable = 8,
        AgentSelected = 9
    }

    public enum VendorTypesEnum
    {
        Customer = 1,
        Agent = 2,
    }

    public enum LoginDeviceTypeEnum
    {
        Web = 1,
        Droid = 2,
        Ios = 3
    }

    public enum MasterDataTypesEnum
    {
        Countries = 1,
        Languages = 2,
        TimeZones = 3,
        Ratings = 4,
        Brands = 5,
        Retailers = 6,
        CallStatusTypes = 7
    }

    public class ExcelAttribute : Attribute
    {
        public string Description { get; set; }
        public int OrderId { get; set; }
        public ExcelDataType DataType { get; set; }
        public string PropertyName { get; set; }
        public ExcelAttribute(string description, int orderId, ExcelDataType dataType = ExcelDataType.String)
        {
            Description = description;
            OrderId = orderId;
            DataType = dataType;
        }
    }

    public enum ExcelDataType
    {
        String,
        Bool,
        DateTime,
        Number
    }
}

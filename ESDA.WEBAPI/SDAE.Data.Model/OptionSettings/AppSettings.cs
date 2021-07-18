using System;

namespace SDAE.Data.Model.OptionSettings
{
    public class AppSettings
    {
        public string BasePathApi { get; set; }
        public bool UseDbTwilio { get; set; }
        public int BaseTwilioAccountId { get; set; }
        public string ConnectImgPath { get; set; }
        public string EndImgPath { get; set; }
        public string IOSAppId { get; set; }
        public int IOSPort { get; set; }
        public string IOSHost { get; set; }
        public string IOSCertificatePath { get; set; }
        public string IOSPassword { get; set; }
        public string DroidAppId { get; set; }

        //Power BI
        public string ApiUrl { get; set; }
        public string ApplicationId { get; set; }
        public Guid GroupId { get; set; }
        public string ReportId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string DashboardReportId { get; set; }
        public string AllStatesAnalysisReportId { get; set; }
        public string SwingStatesAnalysisReportId { get; set; }
        public string DayWiseSentimentReportId { get; set; }
        public string PredictReportId { get; set; }

    }
}

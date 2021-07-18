using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using SDAE.Data.Model.OptionSettings;
using Newtonsoft.Json;
using System.Net.Http;
using SDAE.Common;

namespace SDAE.Api.Controllers
{
    public class ReportsController : BaseApiController
    {
        // public static string _accessToken;
        public string AccessToken { get; set; }
        private readonly AppSettings _appSettings;
        public ReportsController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public static string BaseUrl
        {
            get
            {
                return "https://login.microsoftonline.com/common/oauth2/token";
            }
        }

        [HttpGet("EmbedReport")]
        public async Task<IActionResult> EmbedReport(int reportId)
        {
            AccessToken = await GetAccessToken().ConfigureAwait(false);
            using (var client = new PowerBIClient(new Uri(_appSettings.ApiUrl), new TokenCredentials(AccessToken, "Bearer")))
            {
                Guid groupId = (await client.Groups.GetGroupsAsync().ConfigureAwait(false)).Value.FirstOrDefault().Id;
                if (groupId == Guid.Empty)
                {
                    string message = "No group available, need to create a group and upload a report";
                    Response.Redirect($"Error?message={message}");
                }
                // Report report = (await client.Reports.GetReportsInGroupAsync(groupId).ConfigureAwait(false)).Value.FirstOrDefault();
                Report report = new Report();
                if (reportId == (int)PowerBiReportIdsEnum.DashboardReportId)
                {
                    try { 
                    report = await client.Reports.GetReportInGroupAsync(groupId, Guid.Parse(_appSettings.DashboardReportId)).ConfigureAwait(false);
                    }
                    catch (Exception ex) 
                    { 
                    }
                }
                if (reportId == (int)PowerBiReportIdsEnum.AllStatesAnalysisReportId)
                {
                    report = await client.Reports.GetReportInGroupAsync(groupId, Guid.Parse(_appSettings.AllStatesAnalysisReportId)).ConfigureAwait(false);
                }
                if (reportId == (int)PowerBiReportIdsEnum.DayWiseSentimentReportId)
                {
                    report = await client.Reports.GetReportInGroupAsync(groupId, Guid.Parse(_appSettings.DayWiseSentimentReportId)).ConfigureAwait(false);
                }
                if (reportId == (int)PowerBiReportIdsEnum.SwingStatesAnalysisReportId)
                {
                    report = await client.Reports.GetReportInGroupAsync(groupId, Guid.Parse(_appSettings.SwingStatesAnalysisReportId)).ConfigureAwait(false);
                }
                if (reportId == (int)PowerBiReportIdsEnum.PredictReportId)
                {
                    report = await client.Reports.GetReportInGroupAsync(groupId, Guid.Parse(_appSettings.PredictReportId)).ConfigureAwait(false);
                }
                if (report != null)
                {
                    return Ok(new { token = AccessToken, StatusCode = "200", Successful = false, embedUrl = report.EmbedUrl + "?rs:Command=Render&rc:Toolbar=true", reportId = report.Id });
                }
                else
                {
                    string message = "No report available in workspace with ID " + groupId + ", Please fill a group id with existing report in appsettings.json file";
                    Response.Redirect($"Error?message={message}");
                    return NotFound(new { token = "", embedUrl = "" });
                }
            }
        }

        private async Task<string> GetAccessToken()
        {
            //try
            //{
                var url = BaseUrl;
                var content = new Dictionary<string, string>();
                content["grant_type"] = "password";
                content["resource"] = "https://analysis.windows.net/powerbi/api";
                content["username"] = _appSettings.UserName;
                content["password"] = _appSettings.Password;
                content["client_id"] = _appSettings.ApplicationId;

                var response = await MakeAsyncRequest(url, content).ConfigureAwait(false);
                var tokenresult = response.Content.ReadAsStringAsync().Result;
                var AAD = JsonConvert.DeserializeObject<Models.AAD>(tokenresult);
                 return AAD.AccessToken;
            //}
            //catch (Exception ex)
            //{
            //}
            //return "";
        }

        private static async Task<HttpResponseMessage> MakeAsyncRequest(string url, Dictionary<string, string> content)
        {
            var httpClient = new HttpClient
            {
                Timeout = new TimeSpan(0, 5, 0),
                BaseAddress = new Uri(url)
            };
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type: application/x-www-form-urlencoded", "application/json");
            if (content == null)
            {
                content = new Dictionary<string, string>();
            }
            var encodedContent = new FormUrlEncodedContent(content);
            var result = await httpClient.PostAsync(httpClient.BaseAddress, encodedContent).ConfigureAwait(false);
            return result;
        }
    }
}
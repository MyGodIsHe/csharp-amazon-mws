using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazone.MWS
{
    /// <summary>
    /// Amazon MWS Reports API
    /// </summary>
    public class Reports : MWS
    {
        public override string ACCOUNT_TYPE { get { return "Merchant"; } }

        public TreeWrapper RequestReport(string reportType, string startDate = null, string endDate = null,
            string[] marketplaceIds = null)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "RequestReport"},
                {"ReportType", reportType},
                {"StartDate", startDate},
                {"EndDate", endDate},
            };
            data.Update(EnumerateParam("MarketplaceIdList.Id.", marketplaceIds));
            return MakeRequest(data);
        }

        public TreeWrapper GetReportRequestList(string[] requestIds = null, string[] types = null,
            string[] processingStatuses = null, string maxCount = null,
            string fromDate = null, string toDate = null)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetReportRequestList"},
                {"MaxCount", maxCount},
                {"RequestedFromDate", fromDate},
                {"RequestedToDate", toDate},
            };
            data.Update(EnumerateParam("ReportRequestIdList.Id.", requestIds));
            data.Update(EnumerateParam("ReportTypeList.Type.", types));
            data.Update(EnumerateParam("ReportProcessingStatusList.Status.", processingStatuses));
            return MakeRequest(data);
        }

        public TreeWrapper GetReportCount(string[] reportTypes = null, string acknowledged = null,
            string fromDate = null, string toDate = null)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetReportCount"},
                {"Acknowledged", acknowledged},
                {"AvailableFromDate", fromDate},
                {"AvailableToDate", toDate},
            };
            data.Update(EnumerateParam("ReportTypeList.Type.", reportTypes));
            return MakeRequest(data);
        }

        public TreeWrapper GetReport(string reportId)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetReport"},
                {"ReportId", reportId},
            };
            return MakeRequest(data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Amazone.MWS
{
    /// <summary>
    /// Amazon MWS Feeds API
    /// </summary>
    class Feeds : MWS
    {
        public const string ACCOUNT_TYPE = "Merchant";

        /// <summary>
        /// Uploads a feed ( xml or .tsv ) to the seller's inventory.
        /// Can be used for creating/updating products on amazon.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="feed_type"></param>
        /// <param name="marketplaceids"></param>
        /// <param name="content_type"></param>
        /// <param name="purge"></param>
        /// <returns></returns>
        public TreeWrapper SubmitFeed(string feed, string feedType, string[] marketplaceIDs = null,
            string content_type = "text/xml", string purge = "false")
        {
            var data = new Dictionary<string, string>() {
                {"Action", "SubmitFeed"},
                {"FeedType", feedType},
                {"PurgeAndReplace", purge},
            };
            if (marketplaceIDs != null)
                data.Update(EnumerateParam("MarketplaceIdList.Id.", marketplaceIDs));
            var md = CalcMD5(feed);
            var headers = new Dictionary<HttpRequestHeader, string>(){
                {HttpRequestHeader.ContentMd5, md},
                {HttpRequestHeader.ContentType, content_type},
            };
            return MakeRequest(data, WebRequestMethods.Http.Post, headers, feed);
        }

        public TreeWrapper GetFeedSubmissionList(string[] feedIDs=null, int? maxCount=null,
            string[] feedTypes=null, string[] processingStatuses=null, string fromDate=null, string toDate=null)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetFeedSubmissionList"},
                {"MaxCount", maxCount != null ? maxCount.ToString() : null},
                {"SubmittedFromDate", fromDate},
                {"SubmittedToDate", toDate},
            };
            data.Update(EnumerateParam("FeedSubmissionIdList.Id", feedIDs));
            data.Update(EnumerateParam("FeedTypeList.Type.", feedTypes));
            data.Update(EnumerateParam("FeedProcessingStatusList.Status.", processingStatuses));
            return MakeRequest(data);
        }

        public TreeWrapper GetSubmissionListByNextToken(string token)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetFeedSubmissionListByNextToken"},
                {"NextToken", token},
            };
            return MakeRequest(data);
        }

        public TreeWrapper GetFeedSubmissionCount(string[] feedTypes=null, string[] processingStatuses=null,
            string fromDate=null, string toDate=null)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetFeedSubmissionCount"},
                {"SubmittedFromDate", fromDate},
                {"SubmittedToDate", toDate}
            };
            data.Update(EnumerateParam("FeedTypeList.Type.", feedTypes));
            data.Update(EnumerateParam("FeedProcessingStatusList.Status.", processingStatuses));
            return MakeRequest(data);
        }

        public TreeWrapper CancelFeedSubmissions(string[] feedIDs=null, string[] feedTypes=null,
            string fromDate=null, string toDate=null)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "CancelFeedSubmissions"},
                {"SubmittedFromDate", fromDate},
                {"SubmittedToDate", toDate}
            };
            data.Update(EnumerateParam("FeedSubmissionIdList.Id.", feedIDs));
            data.Update(EnumerateParam("FeedTypeList.Type.", feedTypes));
            return MakeRequest(data);
        }

        public TreeWrapper GetFeedSubmissionResult(string feedID)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetFeedSubmissionResult"},
                {"FeedSubmissionId", feedID}
            };
            return MakeRequest(data);
        }
    }
}

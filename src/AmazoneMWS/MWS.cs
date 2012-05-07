using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Security.Cryptography;

namespace Amazone.MWS
{
    /// <summary>
    /// Base Amazon API class
    /// </summary>
    public class MWS
    {
        /// <summary>
        /// This is used to post/get to the different uris used by amazon per api
        /// ie. /Orders/2011-01-01
        /// All subclasses must define their own URI only if needed
        /// </summary>
        public const string URI = "/";

        /// <summary>
        /// The API version varies in most amazon APIs
        /// </summary>
        public const string VERSION = "2009-01-01";

        /// <summary>
        /// There seem to be some xml namespace issues. therefore every api subclass
        /// is recommended to define its namespace, so that it can be referenced
        /// like so AmazonAPISubclass.NS.
        /// For more information see http://stackoverflow.com/a/8719461/389453
        /// </summary>
        public const string NS = "";

        /// <summary>
        /// # Some APIs are available only to either a "Merchant" or "Seller"
        /// the type of account needs to be sent in every call to the amazon MWS.
        /// This constant defines the exact name of the parameter Amazon expects
        /// for the specific API being used.
        /// All subclasses need to define this if they require another account type
        /// like "Merchant" in which case you define it like so.
        /// ACCOUNT_TYPE = "Merchant"
        /// Which is the name of the parameter for that specific account type.
        /// </summary>
        public const string ACCOUNT_TYPE = "SellerId";

        private string accessKey;
        private string secretKey;
        private string accountId;
        private string domain;
        private string uri;
        private string version;

        public MWS(string accessKey, string secretKey, string accountId,
                   string domain = "https://mws.amazonservices.com",
                   string uri = MWS.URI, string version = MWS.VERSION)
        {
            this.accessKey = accessKey;
            this.secretKey = secretKey;
            this.accountId = accountId;
            this.domain = domain;
            this.uri = uri;
            this.version = version;
        }

        /// <summary>
        /// Make request to Amazon MWS API with these parameters
        /// </summary>
        /// <param name="extraData"></param>
        /// <param name="method"></param>
        /// <param name="extraHeaders"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public TreeWrapper MakeRequest(IDictionary<string, string> extraData,
            string method = WebRequestMethods.Http.Get, IDictionary<HttpRequestHeader, string> extraHeaders = null,
            string body = null)
        {
            var qParams = new Dictionary<string, string>(){
                {"AWSAccessKeyId", this.accessKey},
                {MWS.ACCOUNT_TYPE, this.accountId},
                {"SignatureVersion", "2"},
                {"Timestamp", this.GetTimestamp()},
                {"Version", this.version},
                {"SignatureMethod", "HmacSHA256"},
            };
            qParams.Update(extraData.Where(i => string.IsNullOrEmpty(i.Value)) as Dictionary<string, string>);

            //TODO add encode('utf-8')
            var requestDescription = string.Join("&",
                from param in qParams
                select
                    Uri.EscapeDataString(
                        string.Format("{1}={2}",
                            param.Key, param.Value)));
            var signature = this.CalcSignature(method, requestDescription);
            var url = string.Format("{1}{2}?{3}&Signature={4}",
                this.domain, this.uri, requestDescription, Uri.EscapeDataString(signature));
            var request = HttpWebRequest.Create(url);
            request.Method = method.ToString();
            request.Headers.Add(HttpRequestHeader.UserAgent, "csharp-amazon-mws/0.0.1 (Language=CSharp)");
            if (extraHeaders != null)
                foreach (var x in extraHeaders)
                    request.Headers.Add(x.Key, x.Value);
            if (!string.IsNullOrEmpty(body))
            {
                var dataStream = request.GetRequestStream();
                var bytes = Encoding.UTF8.GetBytes(body);
                dataStream.Write(bytes, 0, body.Length);
                dataStream.Close();
            }
            var response = request.GetResponse();
            var parsedResponse = new TreeWrapper(response.GetResponseStream(), MWS.NS);
            response.Close();
            return parsedResponse;
        }

        /// <summary>
        /// It can return a GREEN, GREEN_I, YELLOW or RED status.
        /// Depending on the status/availability of the API its being called from.
        /// </summary>
        /// <returns></returns>
        public TreeWrapper GetServiceStatus()
        {
            return this.MakeRequest(new Dictionary<string, string>(){{"Action","GetServiceStatus"}});
        }
            
        /// <summary>
        /// Calculate MWS signature to interface with Amazon
        /// </summary>
        /// <param name="method"></param>
        /// <param name="requestDescription"></param>
        /// <returns></returns>
        public string CalcSignature(string method, string requestDescription)
        {
            var sigData = string.Format("{1}\n{2}\n{3}\n{4}",
                method, this.domain.Replace("https://", "").ToLower(), this.uri, requestDescription);
            var sigDataAsBytes = ASCIIEncoding.ASCII.GetBytes(sigData);
            var hmac = new HMACSHA256(ASCIIEncoding.ASCII.GetBytes(this.secretKey));
            return Convert.ToBase64String(hmac.ComputeHash(sigDataAsBytes));
        }

        /// <summary>
        /// Calculates the MD5 encryption for the given string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string CalcMD5(string text)
        {
            return Convert.ToBase64String(
                MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(text))
            ).Trim(new char[] {'\n'});
        }

        /// <summary>
        /// Returns the current timestamp in proper format.
        /// </summary>
        /// <returns></returns>
        public string GetTimestamp()
        {
            return DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'ddTHH':'mm':'ssZ");
        }

        /// <summary>
        /// Builds a dictionary of an enumerated parameter.
        /// Takes any iterable and returns a dictionary.
        /// ie.
        /// EnumerateParam("MarketplaceIdList.Id", new int[] {123, 345, 4343})
        /// returns
        /// {
        ///     MarketplaceIdList.Id.1: 123,
        ///     MarketplaceIdList.Id.2: 345,
        ///     MarketplaceIdList.Id.3: 4343
        /// }
        /// </summary>
        /// <param name="param"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public Dictionary<string, string> EnumerateParam(string param, string[] values)
        {
            var dict = new Dictionary<string, string>();
            if (!param.EndsWith("."))
                param = param + ".";
            for (int i = 0; i < values.Length; i++)
                dict[string.Format("{1}{2}", param, i + 1)] = values[i];
            return dict;
        }
    }
}
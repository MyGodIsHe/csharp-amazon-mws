using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.MWS
{
    /// <summary>
    /// Amazon MWS Sellers API
    /// </summary>
    public class Sellers : MWS
    {
        public override string URI { get { return "/Sellers/2011-07-01"; } }
        public override string VERSION { get { return "2011-07-01"; } }
        public override string NS { get { return "{http://mws.amazonservices.com/schema/Sellers/2011-07-01}"; } }

        public Sellers(string accessKey, string secretKey, string accountId,
            string domain = null, string uri = null, string version = null)
            : base(accessKey, secretKey, accountId, domain, uri, version) { }

        /// <summary>
        /// Returns a list of marketplaces a seller can participate in and
        /// a list of participations that include seller-specific information in that marketplace.
        /// The operation returns only those marketplaces where the seller"s account is in an active state.
        /// </summary>
        /// <returns></returns>
        public TreeWrapper ListMarketplaceParticipations()
        {
            var data = new Dictionary<string, string>() {
                {"Action", "ListMarketplaceParticipations"},
            };
            return MakeRequest(data);
        }

        /// <summary>
        /// Takes a "NextToken" and returns the same information as "list_marketplace_participations".
        /// Based on the "NextToken".
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public TreeWrapper ListMarketplaceParticipationsByNextToken(string token)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "ListMarketplaceParticipations"},
                {"NextToken", token},
            };
            return MakeRequest(data);
        }
    }
}

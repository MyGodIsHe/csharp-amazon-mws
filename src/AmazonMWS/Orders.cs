using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.MWS
{
    /// <summary>
    /// Amazon Orders API
    /// </summary>
    public class Orders : MWS
    {
        public override string URI { get { return "/Orders/2011-01-01"; } }
        public override string VERSION { get { return "2011-01-01"; } }
        public override string NS { get { return "{https://mws.amazonservices.com/Orders/2011-01-01}"; } }

        public Orders(string accessKey, string secretKey, string accountId,
            string domain = null, string uri = null, string version = null)
            : base(accessKey, secretKey, accountId, domain, uri, version) { }

        public TreeWrapper ListOrders(string[] marketplaceIds, string createdAfter=null,
            string createdBefore=null, string lastUpdatedAfter=null,
            string lastUpdatedBefore=null, string[] orderStatus=null, string[] fulfillmentChannels=null,
            string[] paymentMethods = null, string buyerEmail = null, string sellerOrderid = null,
            int maxResults=100)
        {

            var data = new Dictionary<string, string>() {
                {"Action", "ListOrders"},
                {"CreatedAfter", createdAfter},
                {"CreatedBefore", createdBefore},
                {"LastUpdatedAfter", lastUpdatedAfter},
                {"LastUpdatedBefore", lastUpdatedBefore},
                {"BuyerEmail", buyerEmail},
                {"SellerOrderId", sellerOrderid},
                {"MaxResultsPerPage", maxResults.ToString()},
            };
            data.Update(EnumerateParam("OrderStatus.Status.", orderStatus));
            data.Update(EnumerateParam("MarketplaceId.Id.", marketplaceIds));
            data.Update(EnumerateParam("FulfillmentChannel.Channel.", fulfillmentChannels));
            data.Update(EnumerateParam("PaymentMethod.Method.", paymentMethods));
            return MakeRequest(data);
        }

        public TreeWrapper ListOrdersByNextToken(string token)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "ListOrdersByNextToken"},
                {"NextToken", token},
            };
            return MakeRequest(data);
        }

        public TreeWrapper GetOrder(string[] amazonOrderIds)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetOrder"},
            };
            data.Update(EnumerateParam("AmazonOrderId.Id.", amazonOrderIds));
            return MakeRequest(data);
        }

        public TreeWrapper ListOrderItems(string amazonOrderId)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "ListOrderItems"},
                {"AmazonOrderId", amazonOrderId},
            };
            return MakeRequest(data);
        }

        public TreeWrapper ListOrderItemsByNextToken(string token)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "ListOrderItemsByNextToken"},
                {"NextToken", token},
            };
            return MakeRequest(data);
        }
    }
}

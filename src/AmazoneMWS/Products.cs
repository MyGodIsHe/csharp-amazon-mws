using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazone.MWS
{
    /// <summary>
    /// Amazon MWS Products API
    /// </summary>
    public class Products : MWS
    {
        public override string URI { get { return "/Products/2011-10-01"; } }
        public override string VERSION { get { return "2011-10-01"; } }
        public override string NS { get { return "{http://mws.amazonservices.com/schema/Products/2011-10-01}"; } }

        /// <summary>
        /// Returns a list of products and their attributes, ordered by
        /// relevancy, based on a search query that you specify.
        /// Your search query can be a phrase that describes the product
        /// or it can be a product identifier such as a UPC, EAN, ISBN, or JAN.
        /// </summary>
        /// <param name="marketplaceid"></param>
        /// <param name="query"></param>
        /// <param name="contextid"></param>
        /// <returns></returns>
        public TreeWrapper ListMatchingProducts(string marketplaceId, string query, string contextId=null)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "ListMatchingProducts"},
                {"MarketplaceId", marketplaceId},
                {"Query", query},
                {"QueryContextId", contextId},
            };
            return MakeRequest(data);
        }

        /// <summary>
        /// Returns a list of products and their attributes, based on a list of
        /// ASIN values that you specify.
        /// </summary>
        /// <param name="marketplaceId"></param>
        /// <param name="asins"></param>
        /// <returns></returns>
        public TreeWrapper GetMatchingProduct(string marketplaceId, string[] asins)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetMatchingProduct"},
                {"MarketplaceId", marketplaceId},
            };
            data.Update(EnumerateParam("ASINList.ASIN.", asins));
            return MakeRequest(data);
            }

        /// <summary>
        /// Returns the current competitive pricing of a product,
        /// based on the SellerSKU and MarketplaceId that you specify.
        /// </summary>
        /// <param name="marketplaceId"></param>
        /// <param name="skus"></param>
        /// <returns></returns>
        public TreeWrapper GetCompetitivePricingForSku(string marketplaceId, string[] skus)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetCompetitivePricingForSKU"},
                {"MarketplaceId", marketplaceId},
            };
            data.Update(EnumerateParam("SellerSKUList.SellerSKU.", skus));
            return MakeRequest(data);
        }

        /// <summary>
        /// Returns the current competitive pricing of a product,
        /// based on the ASIN and MarketplaceId that you specify.
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public TreeWrapper GetCompetitivePricingForAsin(string marketplaceId, string[] asins)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetCompetitivePricingForASIN"},
                {"MarketplaceId", marketplaceId},
            };
            data.Update(EnumerateParam("ASINList.ASIN.", asins));
            return MakeRequest(data);
        }

        public TreeWrapper GetLowestOfferListingsForSku(string marketplaceId, string[] skus,
            string condition="Any")
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetLowestOfferListingsForSKU"},
                {"MarketplaceId", marketplaceId},
                {"ItemCondition", condition},
            };
            data.Update(EnumerateParam("SellerSKUList.SellerSKU.", skus));
            return MakeRequest(data);
        }

        public TreeWrapper GetLowestOfferListingsForAsin(string marketplaceId, string[] asins,
            string condition="All")
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetLowestOfferListingsForASIN"},
                {"MarketplaceId", marketplaceId},
                {"ItemCondition", condition},
            };
            data.Update(EnumerateParam("ASINList.ASIN.", asins));
            return MakeRequest(data);
        }

        public TreeWrapper GetProductCategoriesForSku(string marketplaceId, string sku)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetProductCategoriesForSKU"},
                {"MarketplaceId", marketplaceId},
                {"SellerSKU", sku},
            };
            return MakeRequest(data);
        }

        public TreeWrapper GetProductCategoriesForAsin(string marketplaceId, string asin)
        {
            var data = new Dictionary<string, string>() {
                {"Action", "GetProductCategoriesForASIN"},
                {"MarketplaceId", marketplaceId},
                {"ASIN", asin},
            };
            return MakeRequest(data);
        }
    }
}

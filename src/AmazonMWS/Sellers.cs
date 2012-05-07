using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.MWS
{
    public class Sellers : MWS
    {
        public Sellers(string accessKey, string secretKey, string accountId,
            string domain = null, string uri = null, string version = null)
            : base(accessKey, secretKey, accountId, domain, uri, version) { }
    }
}

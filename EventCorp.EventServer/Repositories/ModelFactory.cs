using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;

namespace EventCorp.EventServer.Repositories
{
    public class ModelFactory
    {
        #region Field
        private readonly UrlHelper _UrlHelper;
        #endregion

        #region Ctor
        public ModelFactory(HttpRequestMessage request)
        {
            _UrlHelper = new UrlHelper(request);
        }

        #endregion

    }
}

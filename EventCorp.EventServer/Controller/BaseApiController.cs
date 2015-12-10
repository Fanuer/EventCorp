using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventCorp.EventServer.Interfaces;
using EventCorp.EventServer.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace EventCorp.EventServer.Controller
{
    public class BaseApiController : ApiController
    {
        #region Field
        private ModelFactory _modelFactory;
        private IEventServerRepository _rep = null;
        private EventContext _ctx = null;
        #endregion

        #region Ctor
        public BaseApiController()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Singleton Modelfactory
        /// </summary>
        protected ModelFactory AppModelFactory
        {
            get { return _modelFactory ?? (_modelFactory = new ModelFactory(this.Request)); }
        }


        /// <summary>
        /// Repository
        /// </summary>
        internal IEventServerRepository AppRepository
        {
            get { return this._rep ?? Request.GetOwinContext().Get<IEventServerRepository>(); }
        }

        internal EventContext AppContext
        {
            get
            {
                return _ctx ?? Request.GetOwinContext().Get<EventContext>();
            }
        }

        /// <summary>
        /// Current User Id
        /// </summary>
        protected string CurrentUserId
        {
            get { return this.User.Identity.GetUserId(); }
        }
        #endregion


    }
}

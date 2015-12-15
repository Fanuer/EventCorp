using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using EventCorp.EventServer.Entities;
using EventCorps.Helper.Models;

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

        #region Models

        internal EventModel CreateViewModel(Event datamodel, string currentUserId = null)
        {
            if (datamodel == null) { throw new ArgumentNullException("datamodel"); }
            var userCount = datamodel.Subscribers.Count;
            var isBookedOut = userCount >= datamodel.MaxNumberOfParticipants;

            return new EventModel()
            {
                Id = datamodel.Id,
                Name = datamodel.Name,
                Description = datamodel.Description,
                MaxUsers = datamodel.MaxNumberOfParticipants,
                Place = datamodel.Place,
                StartUTC = datamodel.StartTime,
                Type = datamodel.Type,
                IsBookedOut = isBookedOut,
                NumberFree = isBookedOut ? 0 : datamodel.MaxNumberOfParticipants - userCount,
                NumberWaitingList = !isBookedOut ? 0 : userCount - datamodel.MaxNumberOfParticipants,
                Expired = datamodel.StartTime <= DateTime.UtcNow,
                UserHasSubscribed = !String.IsNullOrEmpty(currentUserId) && datamodel.Subscribers.Any(x => x.UserId.ToString().Equals(currentUserId)),
                Url = _UrlHelper.Link("GetEventById", new { id = datamodel.Id })
            };
        }
        #endregion

        internal Event CreateModel(CreateEventModel model, Event datamodel = null)
        {
            if (model == null) { throw new ArgumentNullException("model"); }

            datamodel = datamodel ?? new Event();
            datamodel.StartTime = model.StartUTC;
            datamodel.Description = model.Description;
            datamodel.MaxNumberOfParticipants = model.MaxUsers;
            datamodel.Name = model.Name;
            datamodel.Place = model.Place;
            datamodel.Type = model.Type;

            return datamodel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI;
using EventCorp.EventServer.Entities;
using EventCorps.Helper.Enums;
using EventCorps.Helper.Models;
using Microsoft.AspNet.Identity;
using Swashbuckle.Swagger.Annotations;

namespace EventCorp.EventServer.Controller
{
    /// <summary>
    /// Handles User-based Actions
    /// </summary>
    [Authorize]
    [RoutePrefix("api/events")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "An internal Server error has occured")]
    [SwaggerResponse(HttpStatusCode.Unauthorized, "You are not allowed to receive this resource")]
    public class EventsController : BaseApiController
    {
        /// <summary>
        /// Gets all Events
        /// </summary>
        /// <param name="onlyOpenEvents">true: show only future events, false: show all Events</param>
        /// <param name="onlySubscriped">true: shows only events that the executing user have subscribed to, false: show all</param>
        /// <param name="pageSize">Elements per Page. If this value is lt. 0 all elements will be returned</param>
        /// <param name="page">page number</param>
        /// <param name="searchTerm">searches for events with this value as name or place</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<EventModel>))]
        public async Task<IQueryable<EventModel>> GetEvents(string searchTerm = "", bool onlyOpenEvents = true, bool onlySubscriped = false, int pageSize = 25, int page = 0)
        {
            var searchTermNotSet = String.IsNullOrWhiteSpace(searchTerm);
            if (!searchTermNotSet)
            {
                searchTerm = searchTerm.ToLower();
            }

            var result = await AppContext.Events
                            .Where(x => !onlyOpenEvents || (x.StartTime >= DateTime.UtcNow))
                            .Where(x => searchTermNotSet || (x.Name.ToLower().Equals(searchTerm) && x.Place.ToLower().Equals(searchTerm)))
                            .Where(x => !onlySubscriped || x.Subscribers.Any(sub => sub.UserId.ToString().Equals(CurrentUserId)))
                            .OrderByDescending(x => x.StartTime)
                            .ThenBy(x => x.Name)
                            .Skip(pageSize > 0 && page > 0 ? page * pageSize : 0)
                            .Take(pageSize > 0 ? pageSize : 25)
                            .Include(x => x.Subscribers)
                            .ToListAsync();

            return result.Select(x => AppModelFactory.CreateViewModel(x, CurrentUserId)).AsQueryable();
        }

        /// <summary>
        /// Get an Event by its Id
        /// </summary>
        /// <param name="id">Id of the event</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}", Name = "GetEventById")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EventModel))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> GetEvent(int id)
        {
            var eventResult = await AppRepository.Events.FindAsync(id);
            if (eventResult == null)
            {
                return NotFound();
            }
            return Ok(AppModelFactory.CreateViewModel(eventResult, CurrentUserId));
        }

        /// <summary>
        /// Creates a new Event
        /// </summary>
        /// <param name="model">event data</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateEvent(CreateEventModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var datamodel = this.AppModelFactory.CreateModel(model);
                await this.AppRepository.Events.AddAsync(datamodel);
                var viewmodel = this.AppModelFactory.CreateViewModel(datamodel, CurrentUserId);
                return CreatedAtRoute("GetExerciseById", new { id = viewmodel.Id }, viewmodel);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Updates a given Event
        /// </summary>
        /// <param name="id">Evvent Id</param>
        /// <param name="model">Model data</param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateEvent([FromUri] int id, [FromBody] UpdateEventModel model)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError("id", "The given id have to be the same as in the model");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await this.AppRepository.Events.ExistsAsync(id);

            try
            {
                var orig = await this.AppRepository.Events.FindAsync(id);
                orig = this.AppModelFactory.CreateModel(model, orig);
                await this.AppRepository.Events.UpdateAsync(orig);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!exists)
                {
                    return NotFound();
                }
                return InternalServerError(e);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes an Event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteEvent(int id)
        {
            var eventResult = await this.AppRepository.Events.FindAsync(id);
            if (eventResult == null)
            {
                return NotFound();
            }

            await this.AppRepository.Events.RemoveAsync(eventResult);
            return Ok();
        }

        /// <summary>
        /// Subscribe to an Event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("{id:int}/subscribe")]
        [HttpPost]
        public async Task<IHttpActionResult> Subscribe([FromUri]int id)
        {
            var eventResult = await AppRepository.Events.FindAsync(id);
            if (eventResult == null)
            {
                return NotFound();
            }
            if (eventResult.Subscribers.Any(x => x.UserId.ToString().Equals(CurrentUserId)))
            {
                ModelState.AddModelError("", "You are already subscribed");
            }
            if (eventResult.StartTime <= DateTime.UtcNow)
            {
                ModelState.AddModelError("", "The Event already took place");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = eventResult.MaxNumberOfParticipants > eventResult.Subscribers.Count + 1
                ? SubscriptionType.Subscribed
                : SubscriptionType.WaitingList;
            eventResult.Subscribers.Add(new Subscriber
            {
                Event = eventResult,
                EventId = eventResult.Id,
                Status = result,
                SubscriptionTime = DateTime.UtcNow,
                UserId = new Guid(CurrentUserId)
            });

            await AppRepository.Events.UpdateAsync(eventResult);

            return Ok(result);
        }

        /// <summary>
        /// Remove Subscription from an event
        /// </summary>
        /// <param name="id">id of the event you want to unsubscribe</param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("{id:int}/unsubscribe")]
        [HttpDelete]
        public async Task<IHttpActionResult> Unsubscribe([FromUri]int id)
        {
            var eventResult = await AppRepository.Events.FindAsync(id);
            if (eventResult == null)
            {
                return NotFound();
            }
            var subscriber = eventResult.Subscribers.FirstOrDefault(x => x.UserId.ToString().Equals(CurrentUserId));
            if (subscriber == null)
            {
                ModelState.AddModelError("", "You were not subscribed");
            }
            if (eventResult.StartTime <= DateTime.UtcNow)
            {
                ModelState.AddModelError("", "The Event already took place");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            eventResult.Subscribers.Remove(subscriber);

            var firstWaitListSub = eventResult
                                    .Subscribers
                                    .OrderBy(x => x.SubscriptionTime)
                                    .FirstOrDefault(x => x.Status == SubscriptionType.WaitingList);

            if (firstWaitListSub != null)
            {
                firstWaitListSub.Status = SubscriptionType.Subscribed;
            }
            await AppRepository.Events.UpdateAsync(eventResult);
            return Ok();
        }

        private bool EventContainsSearchTerm(string searchTerm, Event ev)
        {
            var comparer = CultureInfo.CurrentCulture.CompareInfo;

            return comparer.IndexOf(ev.Name, searchTerm, CompareOptions.IgnoreCase) >= 0
                || comparer.IndexOf(ev.Place, searchTerm, CompareOptions.IgnoreCase) >= 0;
        }
    }
}

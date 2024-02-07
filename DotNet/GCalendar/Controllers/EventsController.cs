using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using GCalendar.Contracts;
using GCalendar.Helpers;

namespace GCalendar.Controllers
{
    [ApiController,Route("[controller]")]
    public class EventsController : EventsHelpers
    {
        [HttpGet("test"), HttpGet("~/skill/{controller}/test")]
        public string Test()
        {
            //if the skill is installed as a web application called "gcalendar" in IIS, then both URLs will work:
            //https://example.com/gcalendar/events/test
            //https://example.com/gcalendar/skill/events/test
            System.Diagnostics.Debug.WriteLine("[vertex][Events]Test");
            return "hello world from events.";
        }

        #region Create a Calendar Event
        [HttpPost("create_event"),HttpPost("~/skill/{controller}/create_event")]
        public async Task<ServerResponse> CreateEvent(CreateEventRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Events][CreateEvent]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            try
            {
                response = await AddEvent(Para);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Events][CreateEvent]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Get Calendar Events
        [HttpPost("get_events"), HttpPost("~/skill/{controller}/get_events")]
        public async Task<GetEventsResponse> GetEvents(GetEventsRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Events][GetEvents]");
            var response = new GetEventsResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            try
            {
                response.Events = await ListEvents(Para);
                if (response.Events.Count == 0)
                {
                    response.Message = "No events were found.";
                }
                else
                {
                    response.Message = $"Found {response.Events.Count} event(s).";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Events][GetEvents]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Remove Calendar Events
        [HttpPost("remove_events"), HttpPost("~/skill/{controller}/remove_events")]
        public async Task<ServerResponse> RemoveEvents(RemoveEventsRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Events][RemoveEvents]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            try
            {
                response = await DeleteMultipleEvents(Para);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Events][RemoveEvents]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Get a Recurring Calendar Event
        [HttpPost("get_recurring_event"), HttpPost("~/skill/{controller}/get_recurring_event")]
        public async Task<GetRecurringEventResponse> GetRecurringEvent(GetRecurringEventRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Events][GetRecurringEvent]");
            var response = new GetRecurringEventResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            try
            {
                response.Events = await GetEventInstances(Para);
                if (response.Events.Count == 0)
                {
                    response.Message = "No event instances were found.";
                }
                else
                {
                    response.Message = $"Found {response.Events.Count} event instance(s).";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Events][GetRecurringEvent]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion
    }
}

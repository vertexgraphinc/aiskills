using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GCalendar.Contracts;
using GCalendar.GCalendarClient;

namespace GCalendar.Helpers
{
    public class EventsHelpers : OAuthSession
    {
        int _defaultMaxResults = 5;

        public async Task<ServerResponse> AddEvent(CreateEventRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][AddEvent]");

            var response = new ServerResponse();
            if (!Has(Para.CalendarId))
            {
                throw new Exception("Calender ID is not provided.");
            }

            string addParams = AssembleOptionalParameters(Para.OptionalParams);

            System.Diagnostics.Debug.WriteLine("[vertex][AddEvent]:addParams:" + addParams);

            var result = await Post<Event>($"/{Para.CalendarId}/events?{addParams}", JsonSerializer.Serialize(Para.Event));
            
            if (result == null)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][AddEvent]:Success");
                response.Message = "";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[vertex][AddEvent]:Failure");
                response.Message = "Failed to create an event.";
            }

            return response;
        }

        public async Task<List<Event>> ListEvents(GetEventsRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]");

            var events = new List<Event>();

            if (!Has(Para.CalendarId))
            {
                throw new Exception("Calender ID is not provided.");
            }

            if (!Has(Para.OptionalParams.MaxResults))
            {
                Para.OptionalParams.MaxResults = _defaultMaxResults;
            }

            string searchParams = AssembleOptionalParameters(Para.OptionalParams);

            System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:searchParams:" + searchParams);

            var result = await Get<GCalendarListEventsMessage>($"/{Para.CalendarId}/events?{searchParams}");
            if (result == null)
                return events;

            System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:searchParams:" + searchParams);
            foreach (var item in result.Items)
            {
                
                System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:event:" + JsonSerializer.Serialize(item));
                System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:==================================");

                events.Add(item);
            }
            return events;
        }

        public async Task<Event> GetEvent(GetEventsRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][GetEvent]");

            if (!Has(Para.CalendarId))
            {
                throw new Exception("Calender ID is not provided.");
            }

            if (!Has(Para.EventId))
            {
                throw new Exception("Event ID is not provided.");
            }

            object getPara = new
            {
                maxAttendees = Para.OptionalParams.MaxAttendees,
                timeZone = Para.OptionalParams.TimeZone
            };

            string searchParams = AssembleOptionalParameters(getPara);

            System.Diagnostics.Debug.WriteLine("[vertex][GetEvent]:searchParams:" + searchParams);

            var result = await Get<Event>($"/{Para.CalendarId}/events/{Para.EventId}?{searchParams}");
            if (result != null)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][GetEvent]:searchParams:" + searchParams);
                System.Diagnostics.Debug.WriteLine("[vertex][GetEvent]:event:" + JsonSerializer.Serialize(result));
                System.Diagnostics.Debug.WriteLine("[vertex][GetEvent]:==================================");
            }

            return result;
        }

        public async Task<ServerResponse> DeleteMultipleEvents(RemoveEventsRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveMultipleEvents]");

            var response = new ServerResponse();
            if (!Has(Para.CalendarId))
            {
                throw new Exception("Calender ID is not provided.");
            }

            object getPara = new
            {
                timeMin = Para.OptionalParams.TimeMin,
                timeMax = Para.OptionalParams.TimeMax
            };
            string searchParams = AssembleOptionalParameters(getPara);
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveEvent]:searchParams:" + searchParams);

            var results = await Get<GCalendarListEventsMessage>($"/{Para.CalendarId}/events?{searchParams}");
            if (results == null)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveMultipleEvents]:Failure");
                response.Message = "No events found to be removed.";
                return response;
            }  

            object removePara = new
            {
                sendUpdates = Para.OptionalParams.SendUpdates
            };
            string removeParams = AssembleOptionalParameters(removePara);

            System.Diagnostics.Debug.WriteLine("[vertex][RemoveEvent]:removeParams:" + removeParams);

            var failedCounter = 0;
            foreach (var item in results.Items)
            {
                var result = await Delete($"/{Para.CalendarId}/events/{item.Id}");
                if (!result)
                {
                    failedCounter++;
                }
            }

            if (failedCounter == 0)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveMultipleEvents]:Success");
                response.Message = "";
            } 
            else
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveMultipleEvents]:Failure");
                response.Message = $"Failed to remove {failedCounter.ToString()} event(s).";
            }

            return response;
        }

        public async Task<ServerResponse> DeleteEvent(RemoveEventsRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveEvent]");

            var response = new ServerResponse();
            if (!Has(Para.CalendarId))
            {
                throw new Exception("Calender ID is not provided.");
            }

            if (!Has(Para.EventId))
            {
                throw new Exception("Event ID is not provided.");
            }

            object removePara = new
            {
                sendUpdates = Para.OptionalParams.SendUpdates
            };
            string removeParams = AssembleOptionalParameters(removePara);

            System.Diagnostics.Debug.WriteLine("[vertex][RemoveEvent]:removeParams:" + removeParams);

            var result = await Delete($"/{Para.CalendarId}/events/{Para.EventId}");

            if (result)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveEvent]:Success");
                response.Message = "";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveEvent]:Failure");
                response.Message = "Failed to remove specified event.";
            }

            return response;
        }

        public async Task<List<Event>> GetEventInstances(GetRecurringEventRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]");

            var events = new List<Event>();

            if (!Has(Para.CalendarId))
            {
                throw new Exception("Calender ID is not provided.");
            }

            if (!Has(Para.EventId))
            {
                throw new Exception("Event ID is not provided.");
            }

            if (!Has(Para.OptionalParams.MaxResults))
            {
                Para.OptionalParams.MaxResults = _defaultMaxResults;
            }

            string searchParams = AssembleOptionalParameters(Para.OptionalParams);

            System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]:searchParams:" + searchParams);

            var result = await Get<GCalendarListEventsMessage>($"/{Para.CalendarId}/events/{Para.EventId}/instances?{searchParams}");
            if (result == null)
                return events;

            System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]:searchParams:" + searchParams);
            foreach (var item in result.Items)
            {

                System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]:eventInstance:" + JsonSerializer.Serialize(item));
                System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]:==================================");

                events.Add(item);
            }
            return events;
        }
    }
}

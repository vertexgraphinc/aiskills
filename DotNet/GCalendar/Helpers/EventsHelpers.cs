using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GCalendar.Contracts;
using GCalendar.GCalendarClient;
using Newtonsoft.Json;

namespace GCalendar.Helpers
{
    public class EventsHelpers : OAuthSession
    {
        public async Task<ServerResponse> AddEvent(CreateEventRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][AddEvent]");

            var response = new ServerResponse();

            var result = await Post<Event>($"/primary/events", JsonConvert.SerializeObject(Para.Event, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));
            
            if (Has(result))
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

            if (!Has(Para.TimeMin))
            {
                throw new Exception("Time min is not provided.");
            }

            if (!Has(Para.TimeMax))
            {
                throw new Exception("Time max is not provided.");
            }

            string searchParams = AssembleOptionalParameters(Para);

            System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:searchParams:" + searchParams);

            var result = await Get<GCalendarListEventsMessage>($"/primary/events?{searchParams}");
            if (!(Has(result) && Has(result.Items)))
                return events;

            System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:searchParams:" + searchParams);
            foreach (var item in result.Items)
            {
                
                System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:event:" + System.Text.Json.JsonSerializer.Serialize(item));
                System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:==================================");

                events.Add(item);
            }
            return events;
        }

        public async Task<ServerResponse> DeleteMultipleEvents(RemoveEventsRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveMultipleEvents]");

            var response = new ServerResponse();

            if (!Has(Para.TimeMin))
            {
                throw new Exception("Time min is not provided.");
            }

            if (!Has(Para.TimeMax))
            {
                throw new Exception("Time max is not provided.");
            }

            object getPara = new
            {
                Para.TimeMin,
                Para.TimeMax
            };
            string searchParams = AssembleOptionalParameters(getPara);
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveEvent]:searchParams:" + searchParams);

            var results = await Get<GCalendarListEventsMessage>($"/primary/events?{searchParams}");
            if (!(Has(results) && Has(results.Items)))
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveMultipleEvents]:Failure");
                response.Message = "No events found to be removed.";
                return response;
            }  

            var failedCounter = 0;
            foreach (var item in results.Items)
            {
                if (Has(Para.Email) || Has(Para.DisplayName))
                {
                    if (Has(item.Attendees))
                    {
                        foreach (var attendee in item.Attendees)
                        {
                            if (Has(Para.Email))
                            {
                                if (attendee.Email == Para.Email)
                                {
                                    var result = await Delete($"/primary/events/{item.Id}");
                                    if (!result)
                                    {
                                        failedCounter++;
                                    }
                                    break;
                                }
                            }
                            if (Has(Para.DisplayName))
                            {
                                if (attendee.DisplayName == Para.DisplayName)
                                {
                                    var result = await Delete($"/primary/events/{item.Id}");
                                    if (!result)
                                    {
                                        failedCounter++;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var result = await Delete($"/primary/events/{item.Id}");
                    if (!result)
                    {
                        failedCounter++;
                    }
                    continue;
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
                response.Message = $"Failed to remove {failedCounter} event(s).";
            }

            return response;
        }

        public async Task<List<Event>> GetEventInstances(GetRecurringEventRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]");

            var events = new List<Event>();

            if (!Has(Para.TimeMin))
            {
                throw new Exception("Time min is not provided.");
            }

            if (!Has(Para.TimeMax))
            {
                throw new Exception("Time max is not provided.");
            }

            object getPara = new
            {
                Para.TimeMin,
                Para.TimeMax
            };
            string searchGeneralParams = AssembleOptionalParameters(getPara);
            System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]:searchGeneralParams:" + searchGeneralParams);

            var results = await Get<GCalendarListEventsMessage>($"/primary/events?{searchGeneralParams}");
            if (!(Has(results) && Has(results.Items)))
                return events;

            string searchParams = AssembleOptionalParameters(Para);
            System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]:searchParams:" + searchParams);

            foreach (var item in results.Items)
            {
                if (Has(item.Recurrence))
                {
                    var result = await Get<GCalendarListEventsMessage>($"/primary/events/{item.Id}/instances?{searchParams}");
                    if (!(Has(result) && Has(result.Items)))
                        continue;

                    foreach (var instance in result.Items)
                    {
                        System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]:eventInstance:" + System.Text.Json.JsonSerializer.Serialize(instance));
                        System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]:==================================");

                        events.Add(item);
                    }
                }

            }

            return events;
        }
    }
}

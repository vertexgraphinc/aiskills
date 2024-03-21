using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GCalendar.Contracts;
using GCalendar.GCalendarClient;
using Newtonsoft.Json;
using TimeZoneConverter;

namespace GCalendar.Helpers
{
    public class EventsHelpers : OAuthSession
    {
        public async Task<ServerResponse> AddEvent(CreateEventRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][CreateEvent Params]" + Para.TimeZone + " Time:" + Para.StartDateTime);
            var response = new ServerResponse();
            var attendeeList = new List<Attendee>();

            if (Has(Para.Attendees))
            {
                string[] emailArray = Para.Attendees.Replace(" ", "").Split(',');
                foreach (var email in emailArray)
                {
                    if (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
                    {
                        System.Diagnostics.Debug.WriteLine("[vertex][AttendeeEmailParse]:Sent attendee email in a wrong format");
                        response.Message = "Attendee email in a wrong format";
                    }
                    var newAttendee = new Attendee { Email = email };
                    attendeeList.Add(newAttendee);
                }
            }
            TimeZoneInfo timeZone = TimeZoneInfo.Local;
            if (Has(Para.TimeZone))
            {
                timeZone = TZConvert.GetTimeZoneInfo(Para.TimeZone);
            }
            
            DateTime currentTime = DateTime.Now;
            TimeSpan offset = timeZone.GetUtcOffset(currentTime);

            var eventObject = new Event
            {
                Summary = Para.Summary,
                Description = Para.Description,
                Start = new Start
                {
                    DateTime = ParseDate(Para.StartDateTime, DateTime.MinValue, offset.ToString()),
                    TimeZone = Para.TimeZone
                },
                End = new End
                {
                    DateTime = ParseDate(Para.EndDateTime, DateTime.MinValue, offset.ToString()),
                    TimeZone = Para.TimeZone
                },
                Attendees = attendeeList
        
            };

            
            System.Diagnostics.Debug.WriteLine("[vertex][AddEvent]:test: " + eventObject.ToString());



            var result = await Post<Event>($"/primary/events", JsonConvert.SerializeObject(eventObject, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));

            if (Has(result))
            {
                System.Diagnostics.Debug.WriteLine("[vertex][AddEvent]:Success");
                response.Message = "Event added successfully";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[vertex][AddEvent]:Failure");
                response.Message = "Failed to create an event.";
            }

            return response;
        }

        public async Task<ServerResponse> Update(UpdateEventRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][UppdateEvent] Time Params: " + Para.TimeZone + Para.UpdatedStartDateTime);
            var response = new ServerResponse();

            var req = new GetEventsRequest
            {
                Q = Para.CurrentSummary,
                TimeMin = Para.CurrentStartDateTime,
                TimeMax = Para.CurrentEndDateTime
            };

            var eventList = await ListEvents(req);

            if(eventList.Count > 1)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][UpdateEvent]: More then one event found");
                response.Message = "More then one event found, provide more specific information";
                return response;
            }

            if (eventList.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][UpdateEvent]: Event not found");
                response.Message = "No event was found to update";
                return response;
            }

            var eventToUpdate = eventList[0];

            var attendeeStringList = eventToUpdate.AttendeesEmails?.Split(",");
            var attendeeList = new List<Attendee>();
            foreach (var email in attendeeStringList)
            {
                var newAttendee = new Attendee { Email = email };
                attendeeList.Add(newAttendee);
            }


            if (Has(Para.AttendeesToRemove))
            {
                string[] emailsToRemove = Para.AttendeesToRemove.Replace(" ", "").Split(',');
                attendeeList.RemoveAll(attendee => emailsToRemove.Contains(attendee.Email));
            }

            if (Has(Para.AttendeesToAdd))
            {
                string[] emailsToAdd = Para.AttendeesToAdd.Replace(" ", "").Split(',');
                foreach (var email in emailsToAdd)
                {
                    if (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
                    {
                        System.Diagnostics.Debug.WriteLine("[vertex][UpdateEvent]:Sent attendee email in a wrong format");
                        response.Message = "Attendee email in a wrong format";
                    }
                    var newAttendee = new Attendee { Email = email };
                    attendeeList.Add(newAttendee);
                }
            }

            /*TimeZoneInfo timeZone = TimeZoneInfo.Local;
            if (Has(Para.TimeZone))
            {
                timeZone = TZConvert.GetTimeZoneInfo(Para.TimeZone);
            }

            DateTime currentTime = DateTime.Now;
            TimeSpan offset = timeZone.GetUtcOffset(currentTime);*/

            var eventObject = new Event
            {
                Summary = Para.UpdatedSummary,
                Description = Para.UpdatedSummary,
                Attendees = attendeeList

            };

            if (Has(Para.UpdatedStartDateTime))
            {
                eventObject.Start = new Start
                {
                    DateTime = ParseDate(Para.UpdatedStartDateTime, DateTime.MinValue, "0:00"),
                };
            }

            if (Has(Para.UpdatedEndDateTime))
            {
                eventObject.End = new End
                {
                    DateTime = ParseDate(Para.UpdatedEndDateTime, DateTime.MinValue, "0:00"),
                };
            }


            System.Diagnostics.Debug.WriteLine("[vertex][UpdateEvent]:test: " + eventObject.ToString());



            var result = await Patch($"/primary/events/{Para.Id}", JsonConvert.SerializeObject(eventObject, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));

            if (result)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][UpdateEvent]:Success");
                response.Message = "Event successfully updated";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[vertex][UpdateEvent]:Failure");
                response.Message = "Failed to update an event.";
            }

            return response;
        }

        public async Task<List<SimpleEvent>> ListEvents(GetEventsRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]");

            TimeZoneInfo timeZone = TimeZoneInfo.Local;
  
            DateTime currentTime = DateTime.Now;
            TimeSpan offset = timeZone.GetUtcOffset(currentTime);
            Para.TimeMin = ParseDate(Para.TimeMin, DateTime.Today, offset.ToString());
   
            Para.TimeMax = ParseDate(Para.TimeMax, DateTime.Today.AddDays(7), offset.ToString());
            
            string searchParams = AssembleOptionalParameters(Para);

            System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:searchParams:" + searchParams);

            var result = await Get<GCalendarListEventsMessage>($"/primary/events?{searchParams}");
            if (!(Has(result) && Has(result.Items)))
                return new List<SimpleEvent>();

            System.Diagnostics.Debug.WriteLine("[vertex][ListEvents]:searchParams:" + searchParams);
            var events = new List<SimpleEvent>();
            foreach (var item in result.Items)
            {
                events.Add(SimplifyEvent(item));
            }
            return events;
        }

        public async Task<ServerResponse> DeleteMultipleEvents(RemoveEventsRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveMultipleEvents]");

            var response = new ServerResponse();

            TimeZoneInfo timeZone = TimeZoneInfo.Local;

            DateTime currentTime = DateTime.Now;
            TimeSpan offset = timeZone.GetUtcOffset(currentTime);

            Para.TimeMin = ParseDate(Para.TimeMin, DateTime.Today, offset.ToString());

            Para.TimeMax = ParseDate(Para.TimeMax, DateTime.Today.AddDays(7), offset.ToString());

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

        public async Task<List<SimpleEvent>> GetEventInstances(GetRecurringEventRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][GetEventInstances]");

            var events = new List<SimpleEvent>();

            TimeZoneInfo timeZone = TimeZoneInfo.Local;

            DateTime currentTime = DateTime.Now;
            TimeSpan offset = timeZone.GetUtcOffset(currentTime);

            Para.OriginalStart = ParseDate(Para.OriginalStart, DateTime.Today, offset.ToString());

            Para.TimeMin = ParseDate(Para.TimeMin, DateTime.Today, offset.ToString());

            Para.TimeMax = ParseDate(Para.TimeMax, DateTime.Today.AddDays(7), offset.ToString());

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

                        events.Add(SimplifyEvent(item));
                    }
                }

            }

            return events;
        }
    }
}

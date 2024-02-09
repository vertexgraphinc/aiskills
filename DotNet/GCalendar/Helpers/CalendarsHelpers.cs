using GCalendar.Contracts;
using System;
using System.Threading.Tasks;

namespace GCalendar.Helpers
{
    public class CalendarsHelpers : OAuthSession
    {
        public async Task<ServerResponse> DeleteAllEventsFromCalendar(ClearCalendarRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveAllEventsFromCalendar]");

            var response = new ServerResponse();
            if (!Has(Para.CalendarId))
            {
                throw new Exception("Calender ID is not provided.");
            }

            var result = await Post($"/{Para.CalendarId}/clear");

            if (result)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveAllEventsFromCalendar]:Success");
                response.Message = "";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveAllEventsFromCalendar]:Failure");
                response.Message = "Failed to clear specified calendar.";
            }

            return response;
        }

        public async Task<ServerResponse> DeleteCalendar(RemoveCalendarRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveSingleCalendar]");

            var response = new ServerResponse();
            if (!Has(Para.CalendarId))
            {
                throw new Exception("Calender ID is not provided.");
            }

            var result = await Delete($"/{Para.CalendarId}");

            if (result)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveSingleCalendar]:Success");
                response.Message = "";
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveSingleCalendar]:Failure");
                response.Message = "Failed to remove specified calendar.";
            }

            return response;
        }
    }
}

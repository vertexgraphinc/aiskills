using GCalendar.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Google.Apis.Requests.BatchRequest;
using System.Threading.Tasks;
using System;
using GCalendar.Helpers;

namespace GCalendar.Controllers
{
    public class CalendarsController : CalendarsHelpers
    {
        #region Clear Calendar
        /*[HttpPost("clear"), HttpPost("~/skill/{controller}/clear")]
        public async Task<ServerResponse> ClearCalendar(ClearCalendarRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Calendars][ClearCalendar]");
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
                response = await DeleteAllEventsFromCalendar(Para);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Calendars][ClearCalendar]response:" + JsonConvert.SerializeObject(response));

            return response;
        }*/
        #endregion

        #region Remove Calendar
        /*[HttpPost("remove"), HttpPost("~/skill/{controller}/remove")]
        public async Task<ServerResponse> RemoveCalendar(RemoveCalendarRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Calendars][RemoveCalendar]");
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
                response = await DeleteCalendar(Para);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Calendars][RemoveCalendar]response:" + JsonConvert.SerializeObject(response));

            return response;
        }*/
        #endregion
    }
}

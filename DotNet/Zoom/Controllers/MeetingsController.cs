using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Threading.Tasks;
using Zoom.Contracts;
using Zoom.Helpers;
using Zoom.Interfaces;

namespace Zoom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingsController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpPost("query")]
        public async Task<MeetingsQueryResponse> QueryMeetings(MeetingsQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Query]");
            MeetingsQueryResponse resp = new MeetingsQueryResponse
            {
                Meetings = null
            };
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                resp.Meetings = await _meetingService.QueryMeetings(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Query]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("create")]
        public async Task<MeetingCreateResponse> CreateMeeting(MeetingCreateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Create]");
            MeetingCreateResponse resp = new MeetingCreateResponse
            {
                Meeting = null
            };
            if (request == null)
            {
                Response.StatusCode = 500;
                resp.Message = "Missing parameters";
                return resp;
            }
            if (request.Duration <= 0)
            {
                Response.StatusCode = 500;
                resp.Message = "Duration must be greater than zero minutes";
                return resp;
            }

            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                resp.Meeting = await _meetingService.CreateMeeting(request, token);
                if (resp.Meeting != null)
                {
                    resp.Message = "Meeting created successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to create meeting.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Create]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("update")]
        public async Task<ServerResponse> UpdateMeetings(MeetingsUpdateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Update]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isUpdated = await _meetingService.UpdateMeetings(request, token);
                if (isUpdated)
                {
                    resp.Message = "Meeting updated successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to update meeting.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Update]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("remove")]
        public async Task<ServerResponse> RemoveMeetings(MeetingsRemoveRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Remove]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isUpdated = await _meetingService.RemoveMeetings(request, token);
                if (isUpdated)
                {
                    resp.Message = "Meeting removed successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to remove meeting.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Remove]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("recordings/query")]
        public async Task<MeetingRecordingsQueryResponse> QueryMeetingRecordings(MeetingRecordingsQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Recordings][Query]");
            MeetingRecordingsQueryResponse resp = new MeetingRecordingsQueryResponse
            {
                RecordingFiles = null
            };
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                resp.RecordingFiles = await _meetingService.QueryMeetingRecordings(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Recordings][Query]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("chats/query")]
        public async Task<MeetingChatsQueryResponse> QueryMeetingChats(MeetingChatsQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Chats][Query]");
            MeetingChatsQueryResponse resp = new MeetingChatsQueryResponse
            {
                Chats = null
            };
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                resp.Chats = await _meetingService.QueryMeetingChats(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Meetings][Chats][Query]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }
    }
}

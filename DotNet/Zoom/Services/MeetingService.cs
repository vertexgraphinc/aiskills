using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoom.Contracts;
using Zoom.DTOs;
using Zoom.Helpers;
using Zoom.Interfaces;

namespace Zoom.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly ApiHelper _apiHelper;

        public MeetingService(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        private async Task<ZoomMeetings> QueryRawMeetings(MeetingsQueryRequest request, string token)
        {
            //returns a host's scheduled meetings
            //https://developers.zoom.us/docs/api/rest/reference/zoom-api/methods/#operation/meetings
            string hostsMeetings = "users/me/meetings";
            //https://developers.zoom.us/docs/api/rest/reference/zoom-api/methods/#operation/listUpcomingMeeting
            string userMeetings = "users/me/upcoming_meetings";
            string query = "?type=scheduled";// All valid previous (unexpired) meetings, live meetings, and upcoming scheduled meetings. 

            string from = UtilityHelper.FormatDate(DateTime.Now.AddDays(-7));
            string to = UtilityHelper.FormatDate(DateTime.Now);
            
            if (!string.IsNullOrEmpty(request.From))
            {
                    DateTime BeginDT = DateTime.Parse(request.From);
                    from = UtilityHelper.FormatDate(BeginDT);
            }
            if (!string.IsNullOrEmpty(request.To))
            {
                    DateTime EndDT = DateTime.Parse(request.To).Date;
                    to = UtilityHelper.FormatDate(EndDT);
            }
            query += $"&from={from}&to={to}";
            ZoomMeetings result = new ZoomMeetings();
            result.TotalRecords = 0;
            result.PageNumber = 0;
            result.PageSize = 0;
            result.NextPageToken = "";
            result.Meetings = new List<ZoomMeeting>();
            ZoomMeetings tmp = await _apiHelper.Get<ZoomMeetings>(hostsMeetings + query, token);
            if (tmp != null && tmp.Meetings != null && tmp.Meetings.Count > 0)
            {
                result.Meetings.AddRange(tmp.Meetings);
            }
            await Task.Delay(1000);
            tmp = await _apiHelper.Get<ZoomMeetings>(userMeetings + query, token);
            if (tmp != null && tmp.Meetings != null && tmp.Meetings.Count > 0)
            {
                result.Meetings.AddRange(tmp.Meetings);
            }
            result.Meetings = result.Meetings.Where(meeting => (
                string.IsNullOrEmpty(request.Topic) || 
                string.IsNullOrEmpty(request.Description) ||
                (!string.IsNullOrEmpty(request.Description) && !string.IsNullOrEmpty(meeting.Agenda) && meeting.Agenda.Contains(request.Description)) ||
                (!string.IsNullOrEmpty(request.Topic) && !string.IsNullOrEmpty(meeting.Topic) && meeting.Topic.Contains(request.Topic)))
            ).ToList();
            if(result.Meetings.Count > 0)
            {
                result.TotalRecords = result.Meetings.Count;
                result.PageNumber = 1;
                result.PageSize = 1;
            }
            return result;
        }

        public async Task<List<MeetingResponse>> QueryMeetings(MeetingsQueryRequest request, string token)
        {
            //https://developers.zoom.us/docs/api/rest/reference/zoom-api/methods/#operation/meetings
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][QueryMeetings]");

                ZoomMeetings meetings = await QueryRawMeetings(request, token);
                if (meetings == null || meetings.Meetings == null)
                    return new List<MeetingResponse>();

                var tasks = meetings.Meetings.Select(async meeting => {
                    ZoomUser host = await _apiHelper.Get<ZoomUser>($"users/{meeting.HostId}", token);
                    if (host == null)
                        throw new Exception("Host not found.");

                    return new MeetingResponse
                    {
                        Description = meeting.Agenda,
                        CreatedAt = meeting.CreatedAt,
                        Duration = meeting.Duration,
                        HostEmail = host.Email,
                        JoinUrl = meeting.JoinUrl,
                        StartTime = meeting.StartTime,
                        Topic = meeting.Topic,
                        Type = meeting.Type
                    };
                });
                List<MeetingResponse> results = (await Task.WhenAll(tasks)).ToList();

                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][QueryMeetings]return:" + JsonConvert.SerializeObject(results));
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<MeetingResponse> CreateMeeting(MeetingCreateRequest request, string token)
        {
            //https://developers.zoom.us/docs/api/rest/reference/zoom-api/methods/#operation/meetingCreate
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][CreateMeeting]");

                if (string.IsNullOrEmpty(request.MemberEmails))
                    throw new Exception("Member emails are not specified.");

                request.StartTime = UtilityHelper.TryConvertToZoomDate(request.StartTime);
                string url = "users/me/meetings";
                object body = new
                {
                    agenda = request.Description,
                    topic = request.Topic,
                    start_time = request.StartTime,
                    duration = request.Duration > 0 ? request.Duration : 30,
                    meeting_invitees = request.MemberEmails.Replace(" ", "").Split(",").Select(email => new
                    {
                        email
                    }).ToList(),
                    type = 2, //scheduled meeting
                    auto_recording = request.AutoRecording,
                    settings = new
                    {
                        registrants_confirmation_email = true,
                        registrants_email_notification = true
                    }
                };
                ZoomMeeting meetingCreated = await _apiHelper.Post<ZoomMeeting>(url, body, token);
                if (meetingCreated == null || meetingCreated.Id == 0L)
                    throw new Exception("Meeting failed to create.");

                ZoomUser host = await _apiHelper.Get<ZoomUser>($"users/{meetingCreated.HostId}", token);
                if (host == null)
                    throw new Exception("Host not found.");

                MeetingResponse result = new MeetingResponse
                {
                    Description = meetingCreated.Agenda,
                    CreatedAt = meetingCreated.CreatedAt,
                    Duration = meetingCreated.Duration,
                    HostEmail = host.Email,
                    JoinUrl = meetingCreated.JoinUrl,
                    StartTime = meetingCreated.StartTime,
                    Topic = meetingCreated.Topic,
                    Type = meetingCreated.Type
                };

                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][CreateMeeting]return:" + JsonConvert.SerializeObject(result));
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateMeetings(MeetingsUpdateRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][UpdateMeetings]");

                if (string.IsNullOrEmpty(request.UpdatedDescription) && string.IsNullOrEmpty(request.UpdatedTopic) && string.IsNullOrEmpty(request.UpdatedStartTime) && string.IsNullOrEmpty(request.UpdatedDuration))
                    throw new Exception("One or more updating parameters are not specified.");
                
                ZoomMeetings meetings = await QueryRawMeetings(new MeetingsQueryRequest
                {
                    Topic = request.Topic,
                    Description = request.Description,
                    From = request.From,
                    To = request.To
                }, token);
                if (meetings == null || meetings.Meetings == null)
                    throw new Exception("Meetings not found.");

                var tasks = meetings.Meetings.Select(async meeting =>
                {
                    string urlQuery = $"meetings/{meeting.Id}";
                    object body = new
                    {
                        agenda = request.UpdatedDescription,
                        topic = request.UpdatedTopic,
                        start_time = request.UpdatedStartTime,
                        duration = request.UpdatedDuration
                    };
                    return await _apiHelper.Patch(urlQuery, body, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more meetings failed to update.");

                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][UpdateMeetings]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> RemoveMeetings(MeetingsRemoveRequest request, string token)
        {
            //https://developers.zoom.us/docs/api/rest/reference/zoom-api/methods/#operation/meetingDelete
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][RemoveMeetings]");

                ZoomMeetings meetings = await QueryRawMeetings(new MeetingsQueryRequest
                {
                    Topic = request.Topic,
                    Description = request.Description,
                    From = request.From,
                    To = request.To
                }, token);
                if (meetings == null || meetings.Meetings == null)
                    throw new Exception("Meetings not found.");

                var tasks = meetings.Meetings.Select(async meeting =>
                {
                    string urlQuery = $"meetings/{meeting.Id}";
                    return await _apiHelper.Delete(urlQuery, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more meetings failed to remove.");

                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][RemoveMeetings]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<MeetingRecordingResponse>> QueryMeetingRecordings(MeetingRecordingsQueryRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][QueryMeetingRecordings]");

                ZoomMeetings meetings = await QueryRawMeetings(new MeetingsQueryRequest
                {
                    Topic = request.Topic,
                    Description = request.Description,
                    From = request.From,
                    To = request.To
                }, token);
                if (meetings == null || meetings.Meetings == null)
                    throw new Exception("Meetings not found.");

                var tasks = meetings.Meetings.Select(async meeting =>
                {
                    string urlQuery = $"meetings/{meeting.Id}/recordings";
                    ZoomRecordings recordings = await _apiHelper.Get<ZoomRecordings>(urlQuery, token);
                    if (recordings.RecordingFiles == null || !recordings.RecordingFiles.Any())
                        return new List<MeetingRecordingResponse>();

                    recordings.RecordingFiles = recordings.RecordingFiles.Where(recording => string.IsNullOrEmpty(request.FileType) || recording.FileType == request.FileType).ToList();
                    List<MeetingRecordingResponse> recordingResponses = new List<MeetingRecordingResponse>();
                    foreach(ZoomRecording recording in recordings.RecordingFiles)
                    {
                        recordingResponses.Append(new MeetingRecordingResponse
                        {
                            MeetingTopic = meeting.Topic,
                            MeetingType = meeting.Type,
                            MeetingStartTime = meeting.StartTime,
                            MeetingDuration = meeting.Duration,
                            DownloadUrl = recording.DownloadUrl,
                            FilePath = recording.FilePath,
                            FileSize = recording.FileSize,
                            FileType = recording.FileType,
                            FileExtension = recording.FileExtension,
                            PlayUrl = recording.PlayUrl,
                            RecordingEnd = recording.RecordingEnd,
                            RecordingStart = recording.RecordingStart,
                            RecordingType = recording.RecordingType,
                            Status = recording.Status
                        });
                    }
                    return recordingResponses;
                });
                List<MeetingRecordingResponse> results = (await Task.WhenAll(tasks)).SelectMany(recordingResponse => recordingResponse).ToList();

                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][QueryMeetingRecordings]return:" + JsonConvert.SerializeObject(results));
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<MeetingChatResponse>> QueryMeetingChats(MeetingChatsQueryRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][QueryMeetingChats]");

                ZoomMeetings meetings = await QueryRawMeetings(new MeetingsQueryRequest
                {
                    Topic = request.Topic,
                    Description = request.Description,
                    From = request.From,
                    To = request.To
                }, token);
                if (meetings == null || meetings.Meetings == null)
                    throw new Exception("Meetings not found.");

                var tasks = meetings.Meetings.Select(async meeting =>
                {
                    string urlQuery = $"meetings/{meeting.Id}/recordings";
                    ZoomRecordings recordings = await _apiHelper.Get<ZoomRecordings>(urlQuery, token);
                    if (recordings.RecordingFiles == null || !recordings.RecordingFiles.Any())
                        return new List<MeetingChatResponse>();

                    recordings.RecordingFiles = recordings.RecordingFiles.Where(recording => recording.FileType == "CHAT").ToList();
                    if (recordings.RecordingFiles == null || !recordings.RecordingFiles.Any())
                        return new List<MeetingChatResponse>();

                    List<MeetingChatResponse> chatResponses = new List<MeetingChatResponse>();
                    foreach (ZoomRecording recording in recordings.RecordingFiles)
                    {
                        chatResponses.Append(new MeetingChatResponse
                        {
                            MeetingTopic = meeting.Topic,
                            MeetingType = meeting.Type,
                            MeetingStartTime = meeting.StartTime,
                            MeetingDuration = meeting.Duration,
                            DownloadUrl = recording.DownloadUrl
                        });
                    }
                    return chatResponses;
                });
                List<MeetingChatResponse> results = (await Task.WhenAll(tasks)).SelectMany(chatResponse => chatResponse).ToList();

                System.Diagnostics.Debug.WriteLine("[vertex][MeetingService][QueryMeetingChats]return:" + JsonConvert.SerializeObject(results));
                return results;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}

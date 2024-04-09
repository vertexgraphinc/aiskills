using System.Collections.Generic;
using System.Threading.Tasks;
using Zoom.Contracts;

namespace Zoom.Interfaces
{
    public interface IMeetingService
    {
        Task<List<MeetingResponse>> QueryMeetings(MeetingsQueryRequest request, string token);
        
        Task<MeetingResponse> CreateMeeting(MeetingCreateRequest request, string token);

        Task<bool> UpdateMeetings(MeetingsUpdateRequest request, string token);

        Task<bool> RemoveMeetings(MeetingsRemoveRequest request, string token);

        Task<List<MeetingRecordingResponse>> QueryMeetingRecordings(MeetingRecordingsQueryRequest request, string token);

        Task<List<MeetingChatResponse>> QueryMeetingChats(MeetingChatsQueryRequest request, string token);
    }
}

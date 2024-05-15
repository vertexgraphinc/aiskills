namespace Zoom.Constants
{
    public static class APIConstants
    {
        public const string ZoomApiAuthURL = "https://zoom.us/oauth/";
        public const string ZoomApiBaseURL = "https://api.zoom.us/v2/";
        public const string ZoomApiScope = "user:read:user user:read:list_users:admin meeting:read:meeting meeting:write:meeting meeting:update:meeting meeting:delete:meeting cloud_recording:read:list_recording_files meeting:read:list_meetings, meeting:read:list_meetings:admin meeting:read:list_upcoming_meetings meeting:read:list_upcoming_meetings:admin meeting:read:list_past_instances";
    }
}

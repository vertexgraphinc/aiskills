using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class Creator
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("email"), JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("displayName"), JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("self"), JsonPropertyName("self")]
        public bool Self { get; set; }
    }

    public class Organizer
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("email"), JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("displayName"), JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("self"), JsonPropertyName("self")]
        public bool Self { get; set; }
    }

    public class Start
    {
        [JsonProperty("date"), JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonProperty("dateTime"), JsonPropertyName("dateTime")]
        public string DateTime { get; set; }

        [JsonProperty("timeZone"), JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }
    }

    public class End
    {
        [JsonProperty("date"), JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonProperty("dateTime"), JsonPropertyName("dateTime")]
        public string DateTime { get; set; }

        [JsonProperty("timeZone"), JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }
    }

    public class OriginalStartTime
    {
        [JsonProperty("date"), JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonProperty("dateTime"), JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }

        [JsonProperty("timeZone"), JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }
    }

    public class ConferenceSolutionKey
    {
        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Status
    {
        [JsonProperty("statusCode"), JsonPropertyName("statusCode")]
        public string StatusCode { get; set; }
    }

    public class ConferenceSolution
    {
        [JsonProperty("key"), JsonPropertyName("key")]
        public ConferenceSolutionKey Key { get; set; }

        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("iconUri"), JsonPropertyName("iconUri")]
        public string IconUri { get; set; }
    }

    public class CreateRequest
    {
        [JsonProperty("requestId"), JsonPropertyName("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("conferenceSolutionKey"), JsonPropertyName("conferenceSolutionKey")]
        public ConferenceSolutionKey ConferenceSolutionKey { get; set; }

        [JsonProperty("status"), JsonPropertyName("status")]
        public Status Status { get; set; }
    }

    public class EntryPoint
    {
        [JsonProperty("entryPointType"), JsonPropertyName("entryPointType")]
        public string EntryPointType { get; set; }

        [JsonProperty("uri"), JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonProperty("label"), JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonProperty("pin"), JsonPropertyName("pin")]
        public string Pin { get; set; }

        [JsonProperty("accessCode"), JsonPropertyName("accessCode")]
        public string AccessCode { get; set; }

        [JsonProperty("meetingCode"), JsonPropertyName("meetingCode")]
        public string MeetingCode { get; set; }

        [JsonProperty("passcode"), JsonPropertyName("passcode")]
        public string Passcode { get; set; }

        [JsonProperty("password"), JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class ConferenceData
    {
        [JsonProperty("createRequest"), JsonPropertyName("createRequest")]
        public CreateRequest CreateRequest { get; set; }

        [JsonProperty("entryPoints"), JsonPropertyName("entryPoints")]
        public List<EntryPoint> EntryPoints { get; set; }

        [JsonProperty("conferenceSolution"), JsonPropertyName("conferenceSolution")]
        public ConferenceSolution ConferenceSolution { get; set; }

        [JsonProperty("conferenceId"), JsonPropertyName("conferenceId")]
        public string ConferenceId { get; set; }

        [JsonProperty("signature"), JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonProperty("notes"), JsonPropertyName("notes")]
        public string Notes { get; set; }
    }

    public class Gadget
    {
        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("title"), JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonProperty("link"), JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonProperty("iconLink"), JsonPropertyName("iconLink")]
        public string IconLink { get; set; }

        [JsonProperty("width"), JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonProperty("height"), JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonProperty("display"), JsonPropertyName("display")]
        public string Display { get; set; }

        [JsonProperty("preferences"), JsonPropertyName("preferences")]
        public Dictionary<string, string> Preferences { get; set; }
    }

    public class Reminders
    {
        [JsonProperty("useDefault"), JsonPropertyName("useDefault")]
        public bool UseDefault { get; set; }

        [JsonProperty("overrides"), JsonPropertyName("overrides")]
        public List<ReminderOverride> Overrides { get; set; }
    }

    public class ReminderOverride
    {
        [JsonProperty("method"), JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonProperty("minutes"), JsonPropertyName("minutes")]
        public int Minutes { get; set; }
    }

    public class Source
    {
        [JsonProperty("url"), JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonProperty("title"), JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class CustomLocation
    {
        [JsonProperty("label"), JsonPropertyName("label")]
        public string Label { get; set; }
    }

    public class OfficeLocation
    {
        [JsonProperty("buildingId"), JsonPropertyName("buildingId")]
        public string BuildingId { get; set; }

        [JsonProperty("floorId"), JsonPropertyName("floorId")]
        public string FloorId { get; set; }

        [JsonProperty("floorSectionId"), JsonPropertyName("floorSectionId")]
        public string FloorSectionId { get; set; }

        [JsonProperty("deskId"), JsonPropertyName("deskId")]
        public string DeskId { get; set; }

        [JsonProperty("label"), JsonPropertyName("label")]
        public string Label { get; set; }
    }

    public class WorkingLocationProperties
    {
        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("homeOffice"), JsonPropertyName("homeOffice")]
        public bool HomeOffice { get; set; }

        [JsonProperty("customLocation"), JsonPropertyName("customLocation")]
        public CustomLocation CustomLocation { get; set; }

        [JsonProperty("officeLocation"), JsonPropertyName("officeLocation")]
        public OfficeLocation OfficeLocation { get; set; }
    }

    public class OutOfOfficeProperties
    {
        [JsonProperty("autoDeclineMode"), JsonPropertyName("autoDeclineMode")]
        public string AutoDeclineMode { get; set; }

        [JsonProperty("declineMessage"), JsonPropertyName("declineMessage")]
        public string DeclineMessage { get; set; }
    }

    public class FocusTimeProperties
    {
        [JsonProperty("autoDeclineMode"), JsonPropertyName("autoDeclineMode")]
        public string AutoDeclineMode { get; set; }

        [JsonProperty("declineMessage"), JsonPropertyName("declineMessage")]
        public string DeclineMessage { get; set; }

        [JsonProperty("chatStatus"), JsonPropertyName("chatStatus")]
        public string ChatStatus { get; set; }
    }

    public class Attachment
    {
        [JsonProperty("fileUrl"), JsonPropertyName("fileUrl")]
        public string FileUrl { get; set; }

        [JsonProperty("title"), JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonProperty("mimeType"), JsonPropertyName("mimeType")]
        public string MimeType { get; set; }

        [JsonProperty("iconLink"), JsonPropertyName("iconLink")]
        public string IconLink { get; set; }

        [JsonProperty("fileId"), JsonPropertyName("fileId")]
        public string FileId { get; set; }
    }

    public class ExtendedProperties
    {
        [JsonProperty("private"), JsonPropertyName("private")]
        public Dictionary<string, string> Private { get; set; }

        [JsonProperty("shared"), JsonPropertyName("shared")]
        public Dictionary<string, string> Shared { get; set; }
    }

    public class Attendee
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("email"), JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("displayName"), JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("organizer"), JsonPropertyName("organizer")]
        public bool Organizer { get; set; }

        [JsonProperty("self"), JsonPropertyName("self")]
        public bool Self { get; set; }

        [JsonProperty("resource"), JsonPropertyName("resource")]
        public bool Resource { get; set; }

        [JsonProperty("optional"), JsonPropertyName("optional")]
        public bool Optional { get; set; }

        [JsonProperty("responseStatus"), JsonPropertyName("responseStatus")]
        public string ResponseStatus { get; set; }

        [JsonProperty("comment"), JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonProperty("additionalGuests"), JsonPropertyName("additionalGuests")]
        public int AdditionalGuests { get; set; }
    }

    public class Event
    {
        [JsonProperty("kind"), JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag"), JsonPropertyName("etag")]
        public string ETag { get; set; }

        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("status"), JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("htmlLink"), JsonPropertyName("htmlLink")]
        public string HtmlLink { get; set; }

        [JsonProperty("created"), JsonPropertyName("created")]
        public DateTime? Created { get; set; }

        [JsonProperty("updated"), JsonPropertyName("updated")]
        public DateTime? Updated { get; set; }

        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("location"), JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonProperty("colorId"), JsonPropertyName("colorId")]
        public string ColorId { get; set; }

        [JsonProperty("creator"), JsonPropertyName("creator")]
        public Creator Creator { get; set; }

        [JsonProperty("organizer"), JsonPropertyName("organizer")]
        public Organizer Organizer { get; set; }

        [JsonProperty("start"), JsonPropertyName("start")]
        public Start Start { get; set; }

        [JsonProperty("end"), JsonPropertyName("end")]
        public End End { get; set; }

        [JsonProperty("endTimeUnspecified"), JsonPropertyName("endTimeUnspecified")]
        public bool? EndTimeUnspecified { get; set; }

        [JsonProperty("recurrence"), JsonPropertyName("recurrence")]
        public List<string> Recurrence { get; set; }

        [JsonProperty("recurringEventId"), JsonPropertyName("recurringEventId")]
        public string RecurringEventId { get; set; }

        [JsonProperty("originalStartTime"), JsonPropertyName("originalStartTime")]
        public OriginalStartTime OriginalStartTime { get; set; }

        [JsonProperty("transparency"), JsonPropertyName("transparency")]
        public string Transparency { get; set; }

        [JsonProperty("visibility"), JsonPropertyName("visibility")]
        public string Visibility { get; set; }

        [JsonProperty("iCalUID"), JsonPropertyName("iCalUID")]
        public string ICalUID { get; set; }

        [JsonProperty("sequence"), JsonPropertyName("sequence")]
        public int? Sequence { get; set; }

        [JsonProperty("attendees"), JsonPropertyName("attendees")]
        public List<Attendee> Attendees { get; set; }

        [JsonProperty("attendeesOmitted"), JsonPropertyName("attendeesOmitted")]
        public bool? AttendeesOmitted { get; set; }

        [JsonProperty("extendedProperties"), JsonPropertyName("extendedProperties")]
        public ExtendedProperties ExtendedProperties { get; set; }

        [JsonProperty("hangoutLink"), JsonPropertyName("hangoutLink")]
        public string HangoutLink { get; set; }

        [JsonProperty("conferenceData"), JsonPropertyName("conferenceData")]
        public ConferenceData ConferenceData { get; set; }

        [JsonProperty("gadget"), JsonPropertyName("gadget")]
        public Gadget Gadget { get; set; }

        [JsonProperty("anyoneCanAddSelf"), JsonPropertyName("anyoneCanAddSelf")]
        public bool? AnyoneCanAddSelf { get; set; }

        [JsonProperty("guestsCanInviteOthers"), JsonPropertyName("guestsCanInviteOthers")]
        public bool? GuestsCanInviteOthers { get; set; }

        [JsonProperty("guestsCanModify"), JsonPropertyName("guestsCanModify")]
        public bool? GuestsCanModify { get; set; }

        [JsonProperty("guestsCanSeeOtherGuests"), JsonPropertyName("guestsCanSeeOtherGuests")]
        public bool? GuestsCanSeeOtherGuests { get; set; }

        [JsonProperty("privateCopy"), JsonPropertyName("privateCopy")]
        public bool? PrivateCopy { get; set; }

        [JsonProperty("locked"), JsonPropertyName("locked")]
        public bool? Locked { get; set; }

        [JsonProperty("reminders"), JsonPropertyName("reminders")]
        public Reminders Reminders { get; set; }

        [JsonProperty("source"), JsonPropertyName("source")]
        public Source Source { get; set; }

        [JsonProperty("workingLocationProperties"), JsonPropertyName("workingLocationProperties")]
        public WorkingLocationProperties WorkingLocationProperties { get; set; }

        [JsonProperty("outOfOfficeProperties"), JsonPropertyName("outOfOfficeProperties")]
        public OutOfOfficeProperties OutOfOfficeProperties { get; set; }

        [JsonProperty("focusTimeProperties"), JsonPropertyName("focusTimeProperties")]
        public FocusTimeProperties FocusTimeProperties { get; set; }

        [JsonProperty("attachments"), JsonPropertyName("attachments")]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("eventType"), JsonPropertyName("eventType")]
        public string EventType { get; set; }
    }
}

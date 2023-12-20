using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Outlook.DTOs
{
    public class MSGraphMessage
    {
        [JsonPropertyName("@odata.etag")]
        [JsonProperty("@odata.etag")]
        public string ODataEtag { get; set; }

        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("createdDateTime")]
        [JsonProperty("createdDateTime")]
        public DateTimeOffset? CreatedDateTime { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset? LastModifiedDateTime { get; set; }

        [JsonPropertyName("changeKey")]
        [JsonProperty("changeKey")]
        public string ChangeKey { get; set; }

        [JsonPropertyName("categories")]
        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonPropertyName("receivedDateTime")]
        [JsonProperty("receivedDateTime")]
        public DateTimeOffset? ReceivedDateTime { get; set; }

        [JsonPropertyName("sentDateTime")]
        [JsonProperty("sentDateTime")]
        public DateTimeOffset? SentDateTime { get; set; }

        [JsonPropertyName("hasAttachments")]
        [JsonProperty("hasAttachments")]
        public bool? HasAttachments { get; set; }

        [JsonPropertyName("internetMessageId")]
        [JsonProperty("internetMessageId")]
        public string InternetMessageId { get; set; }

        [JsonPropertyName("subject")]
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("bodyPreview")]
        [JsonProperty("bodyPreview")]
        public string BodyPreview { get; set; }

        [JsonPropertyName("importance")]
        [JsonProperty("importance")]
        public string Importance { get; set; }

        [JsonPropertyName("parentFolderId")]
        [JsonProperty("parentFolderId")]
        public string ParentFolderId { get; set; }

        [JsonPropertyName("conversationId")]
        [JsonProperty("conversationId")]
        public string ConversationId { get; set; }

        [JsonPropertyName("conversationIndex")]
        [JsonProperty("conversationIndex")]
        public byte[] ConversationIndex { get; set; }

        [JsonPropertyName("isDeliveryReceiptRequested")]
        [JsonProperty("isDeliveryReceiptRequested")]
        public bool? IsDeliveryReceiptRequested { get; set; }

        [JsonPropertyName("isReadReceiptRequested")]
        [JsonProperty("isReadReceiptRequested")]
        public bool? IsReadReceiptRequestedD { get; set; }

        [JsonPropertyName("isRead")]
        [JsonProperty("isRead")]
        public bool? IsRead { get; set; }

        [JsonPropertyName("isDraft")]
        [JsonProperty("isDraft")]
        public bool? IsDraft { get; set; }

        [JsonPropertyName("webLink")]
        [JsonProperty("webLink")]
        public string WebLink { get; set; }

        [JsonPropertyName("inferenceClassification")]
        [JsonProperty("inferenceClassification")]
        public string InferenceClassification { get; set; }

        [JsonPropertyName("body")]
        [JsonProperty("body")]
        public MSGraphMessageBody Body { get; set; }

        [JsonPropertyName("sender")]
        [JsonProperty("sender")]
        public MSGraphRecipient Sender { get; set; }

        [JsonPropertyName("from")]
        [JsonProperty("from")]
        public MSGraphRecipient From { get; set; }

        [JsonPropertyName("toRecipients")]
        [JsonProperty("toRecipients")]
        public List<MSGraphRecipient> ToRecipients { get; set; }

        [JsonPropertyName("ccRecipients")]
        [JsonProperty("ccRecipients")]
        public List<MSGraphRecipient> CCRecipients { get; set; }

        [JsonPropertyName("bccRecipients")]
        [JsonProperty("bccRecipients")]
        public List<MSGraphRecipient> BCCRecipients { get; set; }

        [JsonPropertyName("replyTo")]
        [JsonProperty("replyTo")]
        public List<MSGraphRecipient> ReplyTo { get; set; }

        [JsonPropertyName("flag")]
        [JsonProperty("flag")]
        public MSGraphFollowUpFlag Flag { get; set; }
    }
}

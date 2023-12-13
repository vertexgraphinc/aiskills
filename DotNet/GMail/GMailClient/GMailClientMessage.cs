using GMail.Contracts;
using GMail.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.GmailClient
{
    public class GMailClientMessage : ExpandoHelpers
    {
        //This class is used to parse a Gmail's message's payload into a temporary message object 
        string _to = "";
        string _from = "";
        string _subject = "";
        string _body = "";    
        string _received = "";
        public string To
        {
            get { return _to; }
            set { _to = Sanitize(value); }
        }
        public string From
        {
            get { return _from; }
            set { _from = Sanitize(value); }
        }
        public string Subject
        {
            get { return _subject; }
            set { _subject = Sanitize(value); }
        }
        public string Body
        {
            get { return _body; }
            set { _body = Sanitize(StripHtmlTags(value)); }
        }               
        public string Received
        {
            get { return _received; }
            set { _received = Sanitize(value); }
        }
        public GMailClientMessage()
        {
        }
        public GMailClientMessage(object payload)
        {
            if (Has(payload))
            {
                this.LoadMessage(payload);
            }
            else
            {
                throw new Exception("Could not extract the payload from the email message.");
            }
        }
        public bool LoadHeaderProps(dynamic expando)
        {
            if (expando is List<object>)
            {
                var allHeaders = (List<object>)expando;
                try
                {
                    this.To = ExtractHeaderPropVal(allHeaders, "From");
                    this.From = ExtractHeaderPropVal(allHeaders, "To");
                    this.Subject = ExtractHeaderPropVal(allHeaders, "Subject");   
                    this.Received = ExtractHeaderPropVal(allHeaders, "Date");
                    return true;
                } 
                catch(Exception)
                {
                    throw new Exception("Could not extract the headers from the email message.");
                }
            }
            return false;
        }
        private string ExtractHeaderPropVal(List<object> allHeaders, string headerName)
        {
            foreach (var header in allHeaders)
            {
                if (header is IDictionary<string, object>)
                {
                    var nameValue = (IDictionary<string, object>)header;
                    if (nameValue != null)
                    {
                        if (nameValue.ContainsKey("name") && nameValue.ContainsKey("value") && headerName.Equals(nameValue["name"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            return nameValue["value"].ToString();
                        }
                    }
                }
            }
            return "";
        }
       
        public bool LoadMessage(object payload)
        {
            var pl = GetDict(payload);
            if (pl.ContainsKey("headers"))
            {
                if (!this.LoadHeaderProps(pl["headers"]))
                {
                    throw new Exception("Could not extract the headers from the email message.");
                }
            }
            var decodedBody = new StringBuilder("", 2048);
            if (pl.ContainsKey("parts"))
            {
                //for multi-part messages, extract the body from all the parts
                this.Body = decodedBody.Append(ExtractDecodedTextFromParts((dynamic)pl["parts"])).ToString();
            }
            else
            {
                if (pl.ContainsKey("body"))
                {
                    //if there are no parts, then there should be a body:{side:int,data:txt}
                    //as a child of payload
                    this.Body = ExtractDecodedText(payload);
                }
            }
            return true;
        }
    }
}
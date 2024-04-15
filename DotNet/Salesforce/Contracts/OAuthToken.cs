﻿using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;


namespace Salesforce.Contracts
{
    public class OAuthToken : OAuthError
    {
        [JsonPropertyName("access_token")]
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("authentication_token")]
        [JsonProperty("authentication_token")]
        public string AuthenticationToken { get; set; }

        [JsonPropertyName("refresh_token")]
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("id_token")]
        [JsonProperty("id_token")]
        public string IDToken { get; set; }

        private string _expiresIn = null;

        [JsonPropertyName("expires_in")]
        [JsonProperty("expires_in")]
        public string ExpiresIn
        {
            get
            {
                return _expiresIn;
            }
            set
            {
                //"expires_in: (optional) The duration in seconds of the access token 
                //lifetime if an access token is included.  For example, the value 
                //"3600" denotes that the access token will expire in one hour from the 
                //time the response was generated by the authorization server." 
                _expiresIn = value;
                int expiresSeconds = 0;
                int.TryParse(value, out expiresSeconds);

                _tokenExpiry = DateTime.Now.AddSeconds(expiresSeconds);
            }
        }

        private DateTime _tokenExpiry = DateTime.Now;
        public DateTime TokenExpiry
        {
            get
            {
                return _tokenExpiry;
            }
            set
            {
                _tokenExpiry = value;
            }
        }

        [JsonPropertyName("scope")]
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}

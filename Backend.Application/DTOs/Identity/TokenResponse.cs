using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Application.DTOs.Identity
{
    public class TokenResponse
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public List<string> roles { get; set; }
        public bool isVerified { get; set; }
        public string jwtToken { get; set; }
        public DateTime issuedOn { get; set; }
        public DateTime expiresOn { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
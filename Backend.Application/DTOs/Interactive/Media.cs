using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.DTOs.Interactive
{
    public class Media
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
    }
}
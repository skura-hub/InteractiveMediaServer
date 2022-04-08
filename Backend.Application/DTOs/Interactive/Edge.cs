using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.DTOs.Interactive
{
    public class Edge
    {
        //[JsonPropertyName("id")]
        //public int Id { get; set; }
        //[JsonPropertyName("order")]
        //public int Order { get; set; }
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("end")]
        public int End { get; set; }
        [JsonPropertyName("default")]
        public bool IsDefault { get; set; }
    }
}
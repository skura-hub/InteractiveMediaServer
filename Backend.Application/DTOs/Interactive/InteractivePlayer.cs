using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.DTOs.Interactive
{
    public class InteractivePlayer
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("nodes")]
        public List<Node> Nodes { get; set; }
    }
}

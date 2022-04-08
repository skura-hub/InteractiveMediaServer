using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.DTOs.Interactive
{
    public class Node
    {
        [JsonIgnore]
        public int Id { get; set; }

        //[JsonPropertyName("id")]
        //public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("media")]
        public Media Media { get; set; }
        [JsonPropertyName("hasDefaultConnection")]
        public bool HasDefaultConnection { get; set; }
        [JsonPropertyName("hasMultipleDefaultConnections")]
        public bool HasMultipleDefaultConnections { get; set; }
        [JsonPropertyName("connections")]
        public List<Edge> Edges { get; set; }

    }
}
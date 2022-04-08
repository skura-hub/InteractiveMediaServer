using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Application.DTOs.Wip
{
    public class PlaygroundConnectionResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("source")]
        public string NodeStartingId { get; set; }
        [JsonPropertyName("target")]
        public string NodeEndingId { get; set; }
        [JsonPropertyName("label")]
        public string ShortName { get; set; }
        [JsonPropertyName("animated")]
        public bool IsDefault { get; set; }

    }
}

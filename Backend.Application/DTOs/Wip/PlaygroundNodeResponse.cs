using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Application.DTOs.Wip
{
    public class PlaygroundNodeResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("position")]
        public PositionResponse Position { get; set; }
        [JsonPropertyName("className")]
        public string MediaType { get; set; }
        [JsonPropertyName("data")]
        public NodeDataResponse Data { get; set; }
    }
}

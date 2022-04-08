using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.DTOs.Wip
{
    public class PositionResponse
    {
        [JsonPropertyName("x")]
        public int X { get; set; }
        [JsonPropertyName("y")]
        public int Y { get; set; }
    }
}

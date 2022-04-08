using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.DTOs.Wip
{
    public class NodeDataResponse
    {
        [JsonPropertyName("label")]
        public string Name { get; set; }
    }
}

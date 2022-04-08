using Backend.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Application.DTOs.ArtworkResponse
{
    
    public class ArtworkPostRequest
    {  
        [JsonPropertyName("hashtags")]
        public List <string>? Hashtags { get; set; }
       
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("playgroundId")]
        public int WipArtworkId { get; set; }

        [JsonPropertyName("isPrivate")]
        public bool IsPrivate { get; set; }
    }
}

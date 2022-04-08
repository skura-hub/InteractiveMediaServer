using Backend.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Application.DTOs.ArtworkResponse
{
    
    public class ArtworkResponse
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("authorName")]
        public string AuthorName { get; set; }
       
        [JsonPropertyName("authorId")]
        public string AuthorId { get; set; }
       
        [JsonPropertyName("hashtags")]
        public List <String> Hashtags { get; set; }
       
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}

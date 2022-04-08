using Backend.Application.DTOs.Wip;
using Backend.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IPlaygroundService
    {
        Task<Guid> PublishArtwork(string userId, int wipArtworkId, string title, string description, bool isPrivate, List<String> hashtags);
        Task<string> GeneratePlayerJson(string userId, int wipArtworkId);
        Task<string> GetPlaygroundJson(string userId, int wipArtworkId);

        Task DeleteNode(string userId, int artworkId, int nodeId);
        Task<int> AddNode(string userId, int artworkId, int? mediaId, int x, int y, string? name);

        Task UpdateNodeByName(string userId, int artworkId, int nodeId, string name);
        Task UpdateNodeByPosition(string userId, int artworkId, int nodeId, int x, int y);
        Task UpdateNodeByMedia(string userId, int artworkId, int nodeId, int mediaId);

        Task DeleteMedia(string userId, int mediaId);
        Task<PlaygroundMediaResponse> AddMedia(string userId, string path, string name);

        Task DeleteArtwork(string userId, int artworkId);
        Task<int> AddArtwork(string userId, string title, string? description);
        Task UpdateArtworkByTitle(string userId, int artworkId, string title);
        Task UpdateArtworkByDescription(string userId, int artworkId, string description);

        Task DeleteConnection(string userId, int artworkId, int connectionId);
        Task<int> AddConnection(string userId, int artworkId, int nodeStarting, int nodeEnding, bool isDefault, string shortName, string? longName);
        Task UpdateConnectionByIsDefault(string userId, int artworkId, int connectionId, bool isDefault);
        Task UpdateConnectionByShortName(string userId, int artworkId, int connectionId, string shortName);
        Task UpdateConnectionByLongName(string userId, int artworkId, int connectionId, string longName);
        Task<PlaygroundMediaResponse> AddMediaDocument(string userId, string content, string name);
        Task UpdateMediaDocument(string userId, int mediaId, string content, string name);
    }
}

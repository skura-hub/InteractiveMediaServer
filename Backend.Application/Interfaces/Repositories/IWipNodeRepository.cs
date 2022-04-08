using Backend.Application.DTOs.Interactive;
using Backend.Application.DTOs.Wip;
using Backend.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IWipNodeRepository : IGenericRepository<WipNode>
    {
        Task<WipNode> FindByIdAsync(string userId, int wipArtworkId, int nodeId);
        Task<List<WipNode>> FindAllNodesAsync(string userId, int wipArtworkId);
        Task UpdateByIdAsync(string userId, int wipArtworkId, int nodeId, System.Action<WipNode> updateAction);
        Task DeleteByIdAsync(string userId, int wipArtworkId, int nodeId);
        Task<List<PlaygroundNodeResponse>> FindAllNodesWithMedia(string userId, int wipArtworkId);
        Task<PlaygroundNodeResponse> FindByIdWithMediaAsync(string userId, int wipArtworkId, int nodeId);
        Task<List<Node>> FindAllNodesWithEverything(string userId, int wipArtworkId);
    }
}
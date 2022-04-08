using Backend.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IWipArtworkRepository : IGenericRepository<WipArtwork>
    {
        Task<WipArtwork> FindByIdAsync(string userId, int wipArtworkId);
        Task<List<WipArtwork>> FindAllArtworksAsync(string userId);
        Task UpdateByIdAsync(string userId, int wipArtworkId, System.Action<WipArtwork> updateAction);
        Task DeleteByIdAsync(string userId, int wipArtworkId);
    }
}
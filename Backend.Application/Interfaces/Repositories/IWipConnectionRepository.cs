using Backend.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IWipConnectionRepository : IGenericRepository<WipConnection>
    {
        Task<WipConnection> FindByIdAsync(string userId, int wipArtworkId, int connectionId);
        Task<List<WipConnection>> FindAllConnectionsAsync(string userId, int wipArtworkId);
        Task UpdateByIdAsync(string userId, int wipArtworkId, int connectionId, System.Action<WipConnection> updateAction);
        Task DeleteByIdAsync(string userId, int wipArtworkId, int connectionId);
    }
}
using Backend.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IWipMediaRepository : IGenericRepository<WipMedia>
    {
        Task<WipMedia> FindByIdAsync(string userId, int wipMediaId);
        Task<List<WipMedia>> FindAllMediasAsync(string userId);
        Task DeleteByIdAsync(string userId, int wipMediaId);
        Task<List<WipMedia>> FindAllMediaFromArtwork(string userId, int artworkId);
        Task<List<WipMedia>> FindAllMediaByTypeAsync(string userId, int mediaTypeId);
        Task AddStartingMediaDocument(DateTime createdAt, string userId);

    }
}
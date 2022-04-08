using Backend.Application.DTOs.ArtworkResponse;
using Backend.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IArtworkRepository : IGenericRepository<Artwork>
    {
        //Task<List<Artwork>> FindAllArtworksOwnedByUserAsync(int userOwnerId, int userRequesterId);
        Task<Artwork> FindById(Guid artworkId);
        Task<List<ArtworkResponse>> FindAllArtworksOwnedAsync(string userOwnerId);
        Task<List<ArtworkResponse>> FindAllArtworksByName(string name);
    }
}
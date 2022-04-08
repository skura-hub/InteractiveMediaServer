using Backend.Application.DTOs.Wip;
using Backend.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces
{
    public interface IArtworkService
    {
        Task<Guid> PublishArtwork(Artwork artwork);
        Task<Artwork> GetArtwork(string path);
        Task DeleteArtwork(string userId, string path);
    }
}

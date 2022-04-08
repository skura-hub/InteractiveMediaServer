using Backend.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IViewPermissionForGuestRepository : IGenericRepository<ViewPermissionForGuest>
    {
        Task<ViewPermissionForGuest> FindByPath(string path);
        Task<List<ViewPermissionForGuest>> FindByArtwork(string userId, Guid artworkId);



    }
}
using AutoMapper;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities.Catalog;
using Backend.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class ViewPermissionForGuestRepository : GenericRepository<ViewPermissionForGuest>, IViewPermissionForGuestRepository
    {
        public ViewPermissionForGuestRepository(ApplicationDbContext repository) : base(repository)
        {
        }

        public Task<ViewPermissionForGuest> FindByPath(string path)
        {
            var query =
                from per in Entities
                where per.SecretKey == path
                select per;
            return query.SingleAsync();
        }

        public Task<List<ViewPermissionForGuest>> FindByArtwork(string userId, Guid artworkId)
        {
            var query =
                from per in Entities
                where per.PKeyUserOwner == userId && per.PKeyArtwork == artworkId
                select per;
            return query.ToListAsync();
        }
    }
}
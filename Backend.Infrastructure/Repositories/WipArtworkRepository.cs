using AutoMapper;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities.Catalog;
using Backend.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Backend.Infrastructure.Repositories
{
    public class WipArtworkRepository : GenericRepository<WipArtwork>, IWipArtworkRepository
    {
        public WipArtworkRepository(ApplicationDbContext repository) : base(repository)
        {
           
        }

        private IQueryable<WipArtwork> FindAll(string userId)
        {
            var query =
                from a in Entities
                where a.PKeyUser == userId
                select a;
            return query;
        }

        public Task<WipArtwork> FindByIdAsync(string userId, int wipArtworkId)
        {
            var query =
                from a in Entities
                where a.Id == wipArtworkId && a.PKeyUser == userId
                select a;
            return query.SingleAsync<WipArtwork>();
        }

        public Task<List<WipArtwork>> FindAllArtworksAsync(string userId)
        {
            return FindAll(userId).ToListAsync();
        }

        public async Task UpdateByIdAsync(string userId, int wipArtworkId, System.Action<WipArtwork> updateAction)
        {
            WipArtwork c = await FindByIdAsync(userId, wipArtworkId);
            updateAction(c);
            await UpdateAsync(c);
        }

        public async Task DeleteByIdAsync(string userId, int wipArtworkId)
        {
            WipArtwork c = await FindByIdAsync(userId, wipArtworkId);
            await DeleteAsync(c);
        }



    }
}
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
    public class WipConnectionRepository : GenericRepository<WipConnection>, IWipConnectionRepository
    {
        public WipConnectionRepository(ApplicationDbContext repository) : base(repository)
        {
        }
        private IQueryable<WipConnection> FindAll(string userId, int wipArtworkId)
        {
            var query =
                from wipNodes in Entities
                where wipNodes.PKeyUser == userId && wipNodes.PKeyWipArtwork == wipArtworkId
                select wipNodes;
            return query;
        }

        public Task<WipConnection> FindByIdAsync(string userId, int wipArtworkId, int connectionId)
        {
            var query =
                from c in Entities
                where c.Id == connectionId &&
                   c.PKeyUser == userId && c.PKeyWipArtwork == wipArtworkId
                select c;
            return query.SingleAsync<WipConnection>();
        }

        public Task<List<WipConnection>> FindAllConnectionsAsync(string userId, int wipArtworkId)
        {
            return FindAll(userId, wipArtworkId).ToListAsync();
        }

        public async Task UpdateByIdAsync(string userId, int wipArtworkId, int connectionId, System.Action<WipConnection> updateAction)
        {
            WipConnection c = await FindByIdAsync(userId, wipArtworkId, connectionId);
            updateAction(c);
            await UpdateAsync(c);
        }

        public async Task DeleteByIdAsync(string userId, int wipArtworkId, int connectionId)
        {
            WipConnection c = await FindByIdAsync(userId, wipArtworkId, connectionId);
            await DeleteAsync(c);
        }


    }
}
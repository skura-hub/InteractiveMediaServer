using AutoMapper;
using Backend.Application.Enums;
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
    public class WipMediaRepository : GenericRepository<WipMedia>, IWipMediaRepository
    {
        public WipMediaRepository(ApplicationDbContext repository) : base(repository)
        {

        }

        public Task<WipMedia> FindByIdAsync(string userId, int wipMediaId)
        {
            var query =
                from wipMedia in Entities
                where wipMedia.Id == wipMediaId && wipMedia.PKeyUser == userId
                select wipMedia;

            return query.SingleAsync<WipMedia>();
        }
        public Task<List<WipMedia>> FindAllMediasAsync(string userId)
        {
            var query =
                from wipMedias in Entities
                where wipMedias.PKeyUser == userId
                select wipMedias;

            return query.ToListAsync();
        }

        public Task<List<WipMedia>> FindAllMediaByTypeAsync(string userId, int mediaTypeId)
        {
            var query =
                from wipMedias in Entities
                where wipMedias.PKeyUser == userId && wipMedias.FkeyMediaType == mediaTypeId
                select wipMedias;

            return query.ToListAsync();
        }

        public Task<List<WipMedia>>FindAllMediaFromArtwork(string userId, int artworkId)
        {
            /*
            var query =
                from nodeChild in _dbContext.WipNode
                from user in _dbContext.Users
                join mediaParent in _dbContext.WipMedia
                    on new { nodeChild.FkeyWipMedia, nodeChild.PKeyUser } equals new { mediaParent.Id, mediaParent.PKeyUser } 
                    into details
                from d in details
                where d.
            */
            var query =
                from nodeChild in _dbContext.WipNode
                where nodeChild.PKeyUser == userId && nodeChild.PKeyWipArtwork == artworkId
                select nodeChild.WipMedia;
            return query.ToListAsync();
        }

        public async Task DeleteByIdAsync(string userId, int wipMediaId)
        {
            WipMedia wipMedia = await FindByIdAsync(userId, wipMediaId);
            await DeleteAsync(wipMedia);
        }

        public Task AddStartingMediaDocument(DateTime createdAt, string userId)
        {
            WipMedia media = new()
            {
                Id = 1,
                CreatedAt = createdAt,
                UpdatedAt = createdAt,
                Path = "Dokonaj wyboru, klikając na wyświetlane guziki",
                Name = "Pora zacząć!",
                PKeyUser = userId,
                FkeyMediaType = (int)MediaTypes.DOCUMENT
            };
            return AddAsync(media);
        }


    }
}
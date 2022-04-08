using AutoMapper;
using Backend.Application.DTOs.ArtworkResponse;
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
    public class ArtworkRepository : GenericRepository<Artwork>, IArtworkRepository
    {
        
        public ArtworkRepository(ApplicationDbContext repository) :base(repository)
        {
            
        }
        public Task<List<ArtworkResponse>> FindAllArtworksOwnedAsync(string userOwnerId)
        {
            var query =
                from art in Entities
                where art.FKeyUser == userOwnerId
                select new ArtworkResponse
                {
                    AuthorId = art.FKeyUser,
                    AuthorName = art.User.UserName,
                    Description = art.Description,
                    Title = art.Title,
                    Path = art.isPrivate ? ("s" + art.ViewPermissionForGuests.First().SecretKey) : ("p" + art.Id.ToString())
                };
            return query.ToListAsync();
        }

        public Task<List<ArtworkResponse>> FindAllArtworksByName(string name)
        {
            var query =
                from art in Entities
                where art.isPrivate == false && art.Title.StartsWith(name) 
                select new ArtworkResponse
                {
                    AuthorId = art.FKeyUser,
                    AuthorName = art.User.UserName,
                    Description = art.Description,
                    Title = art.Title,
                    Path = ("p" + art.Id.ToString())
                };
            return query.ToListAsync();
        }

        public Task<Artwork> FindById (Guid artworkId)
        {
            return Entities.Where(p => p.Id == artworkId).SingleAsync();
        } 



    }
}
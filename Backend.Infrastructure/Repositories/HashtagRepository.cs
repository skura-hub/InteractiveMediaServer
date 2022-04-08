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
    public class HashtagRepository : GenericRepository<Hashtag>, IHashtagRepository
    {
        public HashtagRepository(ApplicationDbContext repository) : base(repository)
        {
        }
		
		
        
    }
}
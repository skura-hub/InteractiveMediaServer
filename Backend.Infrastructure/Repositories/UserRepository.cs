using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities.Catalog;
using Backend.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Backend.Application.DTOs.ApplicationUser;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;

namespace Backend.Infrastructure.Repositories
{
    public class UserRepository : IApplicationUserRepository
    {
        readonly private ApplicationDbContext _dbContext;
        readonly private IMapper _mapper;
        public UserRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<PublicUserResponse> GetByIdAsync(string id)
        {
            var user = _dbContext.Set<User>()
                    .Where(u => u.Id == id)
                    .Select(u => _mapper.Map<PublicUserResponse>(u))
                    .SingleAsync();
            return user;
        }
        public Task<PublicUserResponse> GetByUserNameAsync(string username)
        {
            var query =
                from u in _dbContext.Set<User>()
                where u.UserName == username
                select _mapper.Map<PublicUserResponse>(u);
            var user = query.SingleAsync();
            return user;
        }
        public Task<List<PublicUserResponse>> FindAllAsync(string username)
        {
            var query =
                from u in _dbContext.Set<User>()
                where u.UserName.StartsWith(username)
                select _mapper.Map<PublicUserResponse>(u);
            var users = query.ToListAsync();
            return users;
        }

        public Task<List<PublicUserResponse>> GetAllAsync()
        {
            var query =
                from u in _dbContext.Set<User>()
                select _mapper.Map<PublicUserResponse>(u);
            var users = query.ToListAsync();
            return users;
        }
    }
}
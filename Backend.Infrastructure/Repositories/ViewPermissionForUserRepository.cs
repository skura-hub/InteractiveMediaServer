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
    public class ViewPermissionForUserRepository : GenericRepository<ViewPermissionForUser>, IViewPermissionForUserRepository
    {
        public ViewPermissionForUserRepository(ApplicationDbContext repository) : base(repository)
        {
        }
		
		
        
    }
}
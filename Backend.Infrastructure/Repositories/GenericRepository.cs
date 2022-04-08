using AutoMapper;
using Backend.Application.Interfaces.Repositories;
using Backend.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        protected DbSet<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }
    }
}

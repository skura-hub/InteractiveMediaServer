using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T obj);
        Task DeleteAsync(T obj);
    }
}

using Backend.Application.DTOs.ApplicationUser;
using Backend.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IApplicationUserRepository
    {
        Task<PublicUserResponse> GetByIdAsync(string id);
        Task<PublicUserResponse> GetByUserNameAsync(string username);
        Task<List<PublicUserResponse>> FindAllAsync(string username);
        Task<List<PublicUserResponse>> GetAllAsync();


    }
}
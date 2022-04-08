using AutoMapper;
using Backend.Application.Interfaces.Repositories;
using Backend.Infrastructure.DbContexts;
using Backend.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Backend.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            #region Repositories

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IArtworkRepository, ArtworkRepository>();
            services.AddTransient<IWipArtworkRepository, WipArtworkRepository>();
            services.AddTransient<IWipConnectionRepository, WipConnectionRepository> ();
            services.AddTransient<IWipMediaRepository, WipMediaRepository>();
            services.AddTransient<IWipNodeRepository, WipNodeRepository>();
            services.AddTransient<IHashtagRepository, HashtagRepository>();
            services.AddTransient<IApplicationUserRepository, UserRepository>();
            services.AddTransient<IViewPermissionForGuestRepository, ViewPermissionForGuestRepository>();
            services.AddTransient<IViewPermissionForUserRepository, ViewPermissionForUserRepository>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            #endregion Repositories
        }
    }
}
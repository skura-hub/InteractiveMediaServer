using Backend.Api.Extensions;
//using Backend.Api.Middlewares;
using Backend.Application.Extensions;
using Backend.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer();
            services.AddContextInfrastructure(_configuration);
            services.AddPersistenceContexts(_configuration);
            services.AddRepositories();
            services.AddSharedInfrastructure(_configuration);
            services.AddEssentials();
            services.AddControllers();
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.ConfigureSwagger();
            app.UseHttpsRedirection();
            //app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseRouting();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
using ConstructionPMS.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConstructionPMS.Services.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<NotificationService.NotificationService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<TaskService>();
            services.AddScoped<UserService>();

            return services;
        }
    }
}
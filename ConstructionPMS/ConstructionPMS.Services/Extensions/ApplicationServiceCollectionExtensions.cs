using Confluent.Kafka;
using ConstructionPMS.Services;
using ConstructionPMS.Services.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConstructionPMS.Services.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Access the configuration
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            // Register application services
            services.AddScoped<NotificationService.INotificationService, NotificationService.NotificationService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();

            // Register the Kafka producer
            services.AddSingleton<IProducer<Null, string>>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = configuration["Kafka:BootstrapServers"] // Use the configuration value
                };
                return new ProducerBuilder<Null, string>(producerConfig).Build();
            });

            // Register the TokenService and its interface
            services.AddScoped<ITokenService>(provider => new TokenService(config["JwtSettings:SecretKey"])); // Pass your secret key here
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
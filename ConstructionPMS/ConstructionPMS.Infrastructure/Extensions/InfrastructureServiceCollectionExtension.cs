using ConstructionPMS.Domain.Interfaces;
using ConstructionPMS.Infrastructure.Elasticsearch;
using ConstructionPMS.Infrastructure.Kafka;
using ConstructionPMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ConstructionPMS.Infrastructure.Extensions
{
    public static class InfrastructureServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure PostgreSQL
            services.AddDbContext<ConstructionDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Configure Elasticsearch
            var elasticsearchSettings = new ElasticsearchSettings();
            configuration.GetSection("Elasticsearch").Bind(elasticsearchSettings);
            services.AddSingleton(elasticsearchSettings);
            services.AddSingleton<IElasticClient>(new ElasticClient(new ConnectionSettings(new Uri(elasticsearchSettings.Url))
                .DefaultIndex(elasticsearchSettings.IndexName)));

            // Register repositories
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            // Register Kafka producer and consumer
            services.AddSingleton<IKafkaProducerService>(provider => new KafkaProducer(configuration["Kafka:BootstrapServers"]));

            services.AddSingleton<KafkaProducer>(provider => new KafkaProducer(configuration["Kafka:BootstrapServers"]));
            services.AddSingleton<KafkaConsumer>(provider => new KafkaConsumer(configuration["Kafka:BootstrapServers"], configuration["Kafka:GroupId"]));

            return services;
        }
    }
}
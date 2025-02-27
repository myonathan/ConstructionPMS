using ConstructionPMS.Infrastructure.Elasticsearch;
using ConstructionPMS.Infrastructure.Kafka;
using ConstructionPMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            services.Configure<ElasticsearchSettings>(configuration.GetSection("Elasticsearch"));
            services.AddSingleton<IElasticClient>(provider =>
            {
                var elasticsearchSettings = provider.GetRequiredService<IOptions<ElasticsearchSettings>>().Value;
                return new ElasticClient(new ConnectionSettings(new Uri(elasticsearchSettings.Url))
                    .DefaultIndex(elasticsearchSettings.IndexName));
            });

            // Register repositories
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped < IUserRepository, UserRepository > ();

            // Retrieve Kafka configuration values
            var kafkaSettings = new
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"]
            };

            // Register Kafka producer
            services.AddSingleton<IKafkaProducerService>(provider =>
                new KafkaProducer(kafkaSettings.BootstrapServers));

            // Register Kafka consumer
            services.AddSingleton<IKafkaConsumerService>(provider =>
            {
                var projectRepository = provider.GetRequiredService<IProjectRepository>();
                var elasticClient = provider.GetRequiredService<IElasticClient>();
                return new KafkaConsumer(kafkaSettings.BootstrapServers, kafkaSettings.GroupId, projectRepository, elasticClient);
            });

            return services;
        }
    }
}
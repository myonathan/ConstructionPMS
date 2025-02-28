using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nest;
using ConstructionPMS.Infrastructure.Kafka;
using Microsoft.Extensions.Configuration;
using ConstructionPMS.Infrastructure.Repositories;

namespace ConstructionPMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Access the configuration
                    var configuration = hostContext.Configuration;

                    // Retrieve Kafka configuration values
                    var kafkaSettings = new
                    {
                        BootstrapServers = configuration["Kafka:BootstrapServers"],
                        GroupId = configuration["Kafka:GroupId"]
                    };

                    var serviceProvider = services.BuildServiceProvider();
                    var kafkaConsumer = serviceProvider.GetRequiredService<IKafkaConsumerService>();

                    services.AddHostedService<KafkaConsumerBackgroundService>(provider =>
                    {
                        return new KafkaConsumerBackgroundService(configuration, kafkaConsumer, provider.GetRequiredService<ILogger<KafkaConsumerBackgroundService>>());
                    });
                });
    }
}
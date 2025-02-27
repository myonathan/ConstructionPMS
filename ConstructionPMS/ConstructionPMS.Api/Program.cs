using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nest;
using ConstructionPMS.Infrastructure.Kafka;
using Microsoft.Extensions.Configuration;

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

                    services.AddHostedService<KafkaConsumerBackgroundService>(provider =>
                    {
                        return new KafkaConsumerBackgroundService(configuration, provider.GetRequiredService<ILogger<KafkaConsumerBackgroundService>>());
                    });
                });
    }
}
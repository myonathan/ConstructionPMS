using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nest;

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

                    // Retrieve the BootstrapServers and GroupId values
                    var bootstrapServers = configuration["Kafka:BootstrapServers"];

                    services.AddHostedService<KafkaConsumerBackgroundService>(provider =>
                    {
                        var bootstrapServers = configuration["Kafka:BootstrapServers"]; // Replace with your Kafka server address
                        return new KafkaConsumerBackgroundService(provider.GetRequiredService<ILogger<KafkaConsumerBackgroundService>>(), bootstrapServers);
                    });
                });
    }
}
using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using Nest; // Assuming you are using NEST for Elasticsearch
using ConstructionPMS.Infrastructure.Repositories;

namespace ConstructionPMS.Infrastructure.Kafka
{
    public class KafkaConsumer : IKafkaConsumerService
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly IProjectRepository _projectRepository; // Repository to fetch project data
        private readonly IElasticClient _elasticClient; // Elasticsearch client

        public KafkaConsumer(string bootstrapServers, string groupId, IProjectRepository projectRepository, IElasticClient elasticClient)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Null, string>(config).Build();
            _projectRepository = projectRepository;
            _elasticClient = elasticClient;
        }

        public async Task ConsumeAsync(string topic, CancellationToken cancellationToken)
        {
            _consumer.Subscribe(topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume(cancellationToken);
                    await ProcessMessageAsync(cr.Value);
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }

        private async Task ProcessMessageAsync(string message)
        {
            try
            {
                // Extract the project ID from the message
                var projectId = ExtractProjectId(message);

                // Fetch the project from the primary data store
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project != null)
                {
                    // Index the project in Elasticsearch
                    await _elasticClient.IndexDocumentAsync(project);
                    Console.WriteLine($"Indexed project '{project.ProjectId}' in Elasticsearch.");
                }
                else
                {
                    Console.WriteLine($"Project with ID '{projectId}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        private int ExtractProjectId(string message)
        {
            // Logic to extract the project ID from the message
            // For example, if the message is "Created project: 123456", you would parse it to get 123456
            return int.Parse(message.Split(':')[1].Trim());
        }
    }
}
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using Nest; // Assuming you are using NEST for Elasticsearch
using ConstructionPMS.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration; // Ensure you have this for IConfiguration

namespace ConstructionPMS.Infrastructure.Kafka
{
    public class KafkaConsumer : IKafkaConsumerService
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly IProjectRepository _projectRepository;
        private readonly IElasticClient _elasticClient;
        private readonly string _indexName;

        public KafkaConsumer(string bootstrapServers, string groupId, IProjectRepository projectRepository, IElasticClient elasticClient, string indexName)
        {
            _consumer = CreateConsumer(bootstrapServers, groupId);
            _projectRepository = projectRepository;
            _elasticClient = elasticClient;
            _indexName = indexName; // Accessing the index name from configuration
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error consuming messages: {ex.Message}");
            }
        }

        private IConsumer<Null, string> CreateConsumer(string bootstrapServers, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return new ConsumerBuilder<Null, string>(config).Build();
        }

        private async Task ProcessMessageAsync(string message)
        {
            try
            {
                if (IsDeletionMessage(message))
                {
                    await HandleDeletionAsync(message);
                }
                else if (IsUpdateMessage(message))
                {
                    await HandleUpdateAsync(message);
                }
                else
                {
                    await HandleIndexingAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        private async Task HandleDeletionAsync(string message)
        {
            var projectId = ExtractProjectId(message);
            var documents = await SearchDocumentsByProjectIdAsync(projectId);

            if (documents.Any())
            {
                await DeleteDocumentsAsync(documents);
            }
            else
            {
                Console.WriteLine($"No documents found with ProjectId '{projectId}'.");
            }
        }

        private async Task HandleUpdateAsync(string message)
        {
            var projectId = ExtractProjectId(message);
            var documents = await SearchDocumentsByProjectIdAsync(projectId);
           
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (documents.Any())
            {
                await UpdateDocumentAsync(documents, project);
            }
            else
            {
                Console.WriteLine($"No documents found with ProjectId '{projectId}'.");
            }
        }

        private async Task UpdateDocumentAsync(List<IHit<Project>> documents, Project project)
        {
            var updateTasks = documents.Select(doc => UpdateDocumentAsync(doc.Id, project)); // Use doc.Id to get the document ID
            await Task.WhenAll(updateTasks);
        }

        private async Task HandleIndexingAsync(string message)
        {
            var projectId = ExtractProjectId(message);
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project != null)
            {
                var indexResponse = await _elasticClient.IndexDocumentAsync(project);
                if (indexResponse.IsValid)
                {
                    Console.WriteLine($"Indexed project '{project.ProjectId}' in Elasticsearch.");
                }
                else
                {
                    Console.WriteLine($"Failed to index project '{project.ProjectId}': {indexResponse.DebugInformation}");
                }
            }
            else
            {
                Console.WriteLine($"Project with ID '{projectId}' not found.");
            }
        }

        private async Task<List<IHit<Project>>> SearchDocumentsByProjectIdAsync(int projectId)
        {
            var searchResponse = await _elasticClient.SearchAsync<Project>(s => s
                .Index(_indexName) // Use the index name from configuration
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.ProjectId)
                        .Value(projectId)
                    )
                )
            );

            // Return the hits directly
            return searchResponse.Hits.ToList();
        }

        private async Task DeleteDocumentsAsync(List<IHit<Project>> documents)
        {
            var deleteTasks = documents.Select(doc => DeleteDocumentAsync(doc.Id, doc.Source)); // Use doc.Id to get the document ID
            await Task.WhenAll(deleteTasks);
        }

        private async Task DeleteDocumentAsync(string docId, Project project) // Keep the project parameter
        {
            var deleteResponse = await _elasticClient.DeleteAsync<Project>(docId, d => d.Index(_indexName)); // Specify the index name

            if (deleteResponse.IsValid)
            {
                Console.WriteLine($"Successfully deleted project with ID '{project.ProjectId}'.");
            }
            else
            {
                Console.WriteLine($"Failed to delete project with ID '{project.ProjectId}': {deleteResponse.DebugInformation}");
            }
        }

        private async Task UpdateDocumentAsync(string docId, Project project) // Keep the project parameter
        {
            var updateResponse = await _elasticClient.UpdateAsync<Project>(docId, u => u
                .Index(_indexName) // Specify the index name
                .Doc(project) // Specify the document to update
            );

            if (updateResponse.IsValid)
            {
                Console.WriteLine($"Successfully updated project with ID '{project.ProjectId}'.");
            }
            else
            {
                Console.WriteLine($"Failed to update project with ID '{project.ProjectId}': {updateResponse.DebugInformation}");
            }
        }

        private bool IsDeletionMessage(string message)
        {
            return message.StartsWith("Deleted project:", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsUpdateMessage(string message)
        {
            return message.StartsWith("Updated project:", StringComparison.OrdinalIgnoreCase);
        }

        private int ExtractProjectId(string message)
        {
            var parts = message.Split(':');
            if (parts.Length < 2 || !int.TryParse(parts[1].Trim(), out var projectId))
            {
                throw new FormatException("Invalid message format for project ID extraction.");
            }
            return projectId;
        }
    }
}
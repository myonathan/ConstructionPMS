using ConstructionPMS.Domain.Entities;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Infrastructure.Elasticsearch
{
    public class ProjectElasticsearchRepository
    {
        private readonly IElasticClient _elasticClient;

        public ProjectElasticsearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task AddAsync(Project project)
        {
            await _elasticClient.IndexDocumentAsync(project);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            var searchResponse = await _elasticClient.SearchAsync<Project>(s => s
                .Index("projects")
                .MatchAll());

            return searchResponse.Documents;
        }
    }
}
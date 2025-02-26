using ConstructionPMS.Domain.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructionPMS.Infrastructure.Elasticsearch
{
    public class NotificationElasticsearchRepository
    {
        private readonly IElasticClient _elasticClient;

        public NotificationElasticsearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task AddAsync(Notification notification)
        {
            await _elasticClient.IndexDocumentAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        {
            var searchResponse = await _elasticClient.SearchAsync<Notification>(s => s
                .Index("notifications")
                .Query(q => q
                    .Term(t => t.UserId, userId)
                ));

            return searchResponse.Documents;
        }
    }
}
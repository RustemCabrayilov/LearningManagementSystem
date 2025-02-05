using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using LearningManagementSystem.Application.Abstractions.Services.ElasticService;
using LearningManagementSystem.Application.Abstractions.Services.OCR;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.Extensions.Options;

namespace LearningManagementSystem.Infrastructure.Services.ElasticService;

public class ElasticService : IElasticService
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private readonly ElasticSettings _elasticSettings;

    public ElasticService(
        ElasticsearchClient elasticsearchClient,
        IOptions<ElasticSettings> options)
    {
        _elasticSettings = options.Value;
        _elasticsearchClient = elasticsearchClient;
    }

    public async Task CreateIndexAsync(string indexName)
    {
        if (_elasticsearchClient.Indices.Exists(indexName).Exists)
            throw new Exception("Index already exists");
        await _elasticsearchClient.Indices.CreateAsync(indexName);
    }

    public async ValueTask<bool> AddOrUpdateAsync(UserResponse request)
    {
        var response = await _elasticsearchClient.IndexAsync(request, idx
            => idx.Index(_elasticSettings.DefaultIndex)
                .OpType(OpType.Index));
        return response.IsValidResponse;
    }

    public async ValueTask<bool> AddOrUpdateBulkAsync(List<UserResponse> requests, string indexName)
    {
        var response = await _elasticsearchClient.BulkAsync(b =>
            b.Index(_elasticSettings.DefaultIndex)
                .UpdateMany(requests, (ud, u)
                    => ud.Doc(u).DocAsUpsert(true)));
        return response.IsValidResponse;
    }

    public async Task<UserResponse> Get(string key)
    {
        var response = await _elasticsearchClient.GetAsync<UserResponse>(key, g
            => g.Index(_elasticSettings.DefaultIndex));
        return response.Source;
    }

    public async Task<List<UserResponse?>> GetAll(string? fieldName, string? fieldValue)
    {
        SearchRequest searchRequest = new(_elasticSettings.DefaultIndex)
        {
            Size = 100,
            Sort = new List<SortOptions>
            {
                SortOptions.Field(new Field($"{fieldName}.keyword"),new FieldSort(){Order = SortOrder.Asc}),
            },
            Query = new BoolQuery
            {
                Should = new Query[]
                {
                    new MatchQuery(new Field(fieldName))
                    {
                        Query = fieldValue.ToLower()
                    },
                    new FuzzyQuery(new Field(fieldName))
                    {
                        Value = fieldValue.ToLower()
                    },
                    new WildcardQuery(new Field(fieldName))
                    {
                        Value = $"*{fieldValue.ToLower()}*"
                    }
                }
            }
        };
        SearchResponse<UserResponse> response = await _elasticsearchClient.SearchAsync<UserResponse>(searchRequest);
        return response.Documents.ToList();
    }

    public async ValueTask<bool> Remove(string key)
    {
      var response=await _elasticsearchClient.DeleteAsync<UserResponse>(key,
          d=>d.Index(_elasticSettings.DefaultIndex));
      return response.IsValidResponse;
    }

    public async ValueTask<long?> RemoveAll()
    {
        var response = await _elasticsearchClient.DeleteByQueryAsync<UserResponse>(d
            =>d.Indices(_elasticSettings.DefaultIndex));
        return response.IsValidResponse?response.Deleted:default;
    }
    public async ValueTask<bool> RemoveIndexAsync(string indexName)
    {
        // Check if the index exists
        var indexExistsResponse = await _elasticsearchClient.Indices.ExistsAsync(indexName);
        if (!indexExistsResponse.Exists)
        {
            throw new NotFoundException("Index does not exist");
        }
        var deleteResponse = await _elasticsearchClient.Indices.DeleteAsync(indexName);
        return deleteResponse.IsValidResponse;
    }
}
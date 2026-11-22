using System.Text.Json;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace Elastic_lib.Client;

public class ElasticClient
{
    private ElasticsearchClient _client;

    private string _indexName;

    public ElasticClient(string url, string username, string password, string indexName)
    {
        _client = new ElasticsearchClient(new ElasticsearchClientSettings(new Uri(url))
            .Authentication(new BasicAuthentication(username, password)));
        _indexName = indexName;
    }
    
    public async Task IndexDocumentAsync<T>(T item, string id, CancellationToken ct = default)
    {
        var res = await _client.IndexAsync(item, i => i.Index(_indexName).Id(id), ct);
    }
    
    public async Task<List<T>> SearchAsync<T>(string query, string field, CancellationToken ct = default)
    {
        var response = await _client.SearchAsync<T>(s => s
                .Indices(_indexName)
                .Query(q => q.Match(m => m.Field(field).Query(query))),
            ct);

        return response.Documents.ToList();
    }
}
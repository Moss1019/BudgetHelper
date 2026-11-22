using Elastic_lib.Client;

namespace Elastic_libTest.Client;

class TestMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public DateTime Timestamp { get; set; }
    
    public string Message { get; set; }
}

[TestFixture]
public class ElasticClientTest
{
    [Test]
    public async Task IndexAndSearch_SavesToElasticAndReturnsDocument()
    {
        var sut = new ElasticClient("http://localhost:9200", "elastic", "secret123!", "test_index");
        var testMessage = new TestMessage{ Timestamp = DateTime.UtcNow, Message = "test message" };
        
        await sut.IndexDocumentAsync(testMessage, testMessage.Id);
        
        var searchRes = await sut.SearchAsync<TestMessage>("test", "message");
        Assert.That(searchRes.Count, Is.GreaterThan(0));
        Assert.That(searchRes.Where((tm) => tm.Message == testMessage.Message).First(), Is.Not.Null);
    }
}
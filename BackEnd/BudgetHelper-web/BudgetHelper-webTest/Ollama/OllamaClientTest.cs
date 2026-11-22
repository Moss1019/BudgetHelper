using BudgetHelper_web.Ollama;

namespace BudgetHelper_webTest.Ollama;

[TestFixture]
public class OllamaClientTest
{
    [Test]
    public async Task TestCheck_ReturnsTags()
    {
        var sut = new OllamaClient();
        
        Assert.That(sut, Is.Not.Null);
        
        var res = await sut.Check();
        Assert.That(res.IsSuccessful, Is.True);
        Assert.That(res.Value.Count, Is.GreaterThan(0));
        Assert.That(res.Value.Contains("qwen3-vl:30b"), Is.True);
    }
    
    [Test]
    public async Task TestExtractReceipt()
    {
        var sut = new OllamaClient();
        
        Assert.That(sut, Is.Not.Null);

        var fileBuffer = await File.ReadAllBytesAsync("receipt.jpg");
        var base64 = Convert.ToBase64String(fileBuffer).Replace("\r", "").Replace("\n", "");
        
        Console.WriteLine(base64);
        
        var res = await sut.ExtractReceipt(base64);
        
        Assert.That(res.IsSuccessful, Is.True);
        Assert.That(res.Value.Lines.Count, Is.GreaterThan(0));

        foreach (var line in res.Value.Lines)
        {
            Console.WriteLine(line);
        }
    }
}
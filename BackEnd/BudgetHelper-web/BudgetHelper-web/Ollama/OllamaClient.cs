using System.Text.Json;
using System.Text.Json.Serialization;
using BudgetApp_lib.Result;
using BudgetHelper_web.Ollama.Model;

namespace BudgetHelper_web.Ollama;

class OllamaModelDetails
{
    public string Format { get; set; }
    public string[] Families { get; set; }
    public string ParameterSize { get; set; }
    public string QuantizationLevel { get; set; }
}

class OllamaModelInfo
{
    public string Name { get; set; }
    public string Model { get; set; }
    public long Size { get; set; }
    public string Digest { get; set; }
    public OllamaModelDetails Details { get; set; }
}

class OllamaTagsResponse
{
    public OllamaModelInfo[] Models { get; set; }
}

class OllamaChatMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
}

class OllamaChatResponse
{
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("message")]
    public OllamaChatMessage Message { get; set; }
    [JsonPropertyName("done")]
    public bool Done { get; set; }
    [JsonPropertyName("error")]
    public string Error { get; set; }
}

class OllamaRequestMessage
{
    public string Role { get; set; } = "user";
    public string Content { get; set; }
    public string[] Images { get; set; }

    public OllamaRequestMessage(string content, string base64Image)
    {
        Content = content;
        Images = new string[] { base64Image };
    }
}

class OllamaRequest
{
    public string Model { get; set; } = "qwen3-vl:8b";
    public bool Stream { get; set; } = false;
    public string Format { get; set; } = "json";
    public object Options { get; set; } = new { temperature = 0 };
    
    public OllamaRequestMessage[] messages { get; set; } = new OllamaRequestMessage[1];

    public OllamaRequest(string messageContent, string base64Image)
    {
        messages[0] = new OllamaRequestMessage(messageContent, base64Image);
    }
}

public class OllamaClient
{
    private static readonly string EXTRACT_PROMPT =
        "You are my personal budget assistant. Please extract the items that were bought from the receipt. ONLY give the lines (combination of item name and price) in a JSON array. { \"lines\": [ {\"title\": \"ITEM TITLE\", \"price\": \"ITEM PRICE\" } ] }. The item names are in Dutch, so just identify the item and its associated price. Return the prices as JSON numbers, e.g. 0.85 instead of 0,85 (avoid a comma)";

    private HttpClient _client;

    private static readonly JsonSerializerOptions Options = new()
    {
        IncludeFields = true,
        PropertyNameCaseInsensitive = true
    };

    public OllamaClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://localhost:11434");
        _client.Timeout = TimeSpan.FromMinutes(5);
    }

    public async Task<OpResult<List<string>>> Check()
    {
        var res = await _client.GetAsync("api/tags");
        var resMsg = res.EnsureSuccessStatusCode();

        if (resMsg.IsSuccessStatusCode)
        {
            var content = await resMsg.Content.ReadAsStringAsync();
            var tags = JsonSerializer.Deserialize<OllamaTagsResponse>(content);
            var modelNames = tags?.Models?.Select(m => m.Name).ToList() ?? new List<string>();
            return OpResult<List<string>>.Success(modelNames);
        }

        return OpResult<List<string>>.Failed("Failed to retrieve tags");
    }

    public async Task<OpResult<ExtractedReceipt>> ExtractReceipt(string base64Image)
    {
        var req = new OllamaRequest(EXTRACT_PROMPT, base64Image);

        var json = JsonSerializer.Serialize(req);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var res = await _client.PostAsync("api/chat", content);

        if (!res.IsSuccessStatusCode)
        {
            return OpResult<ExtractedReceipt>.Failed($"Failed to make request: {res.StatusCode}");
        }

        var resContent = await res.Content.ReadAsStringAsync();
        var ollamaResponse = JsonSerializer.Deserialize<OllamaChatResponse>(resContent);
        try
        {
            Console.Out.WriteLine(ollamaResponse.Message.Content);
            var extractedReceipt = JsonSerializer.Deserialize<ExtractedReceipt>(
                ollamaResponse.Message.Content, Options);
            return OpResult<ExtractedReceipt>.Success(extractedReceipt);
        }
        catch (Exception ex)
        {
            return OpResult<ExtractedReceipt>.Failed(ex.Message);
        }
    }
}
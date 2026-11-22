using Elastic_lib.Client;
using Logging_lib.Model;
using Messaging_lib.Messaging;
using Messaging_lib.Stream;

var elasticClient = new ElasticClient("http://localhost:9200", "elastic", "secret123!", "app-logging");
var loggingConsumer = new DotPulsarConsumer(
    "pulsar://localhost:6650",
    "budget-tn",
    "budget-ns",
    "logs",
    true,
    async (buffer, ct) =>
    {
        var inStream = new MemoryInputStream(buffer);
        var logMessage = LogMessage.FromInputStream(inStream);
        await elasticClient.IndexDocumentAsync(logMessage, Guid.NewGuid().ToString());
    }
);

loggingConsumer.Start();
var isRunning = true;

while (isRunning)
{
    var input = Console.In.ReadLine();
    if (input != null)
    {
        var cleanedInput = input.Trim();
        switch (cleanedInput)
        {
            case "-q":
                isRunning = false;
                break;
            case "-s":
                Console.Out.WriteLine(loggingConsumer.InError
                    ? $"Consumer error {loggingConsumer.Error}"
                    : $"Consumer green");
                break;
        }
    }
}

await loggingConsumer.Stop();
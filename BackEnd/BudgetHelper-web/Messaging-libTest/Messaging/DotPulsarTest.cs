using Messaging_lib.Messaging;
using Messaging_lib.Stream;

namespace Messaging_libTest.Messaging;

public class DotPulsarTest
{
    private DotPulsarProducer _producer;

    private DotPulsarConsumer _consumer;
    
    [SetUp]
    public void Setup()
    {
        _producer = new DotPulsarProducer("pulsar://localhost:6650", 
            "test-tenant",
            "test-ns", 
            "test-topic-1", 
            true);
        _consumer = new DotPulsarConsumer("pulsar://localhost:6650", 
            "test-tenant", 
            "test-ns", 
            "test-topic-1", 
            true,
            (buffer, ct) =>
            {
                var memIn = new MemoryInputStream(buffer);
                return Task.CompletedTask;
            });
    }

    [TearDown]
    public async Task TearDown()
    {
        await _producer.DisposeAsync();
        await _consumer.DisposeAsync();
    }

    [Test]
    public async Task ProducerSendMessage_ArrivesInPulsar()
    {
        var testMessage = "Hello world";
        var mOut = new MemoryOutputStream();
        mOut.Write(testMessage);
        
        Assert.That(_producer.InError, Is.False);

        var sendRes = await _producer.Send(mOut.GetBuffer());
        
        Assert.That(sendRes, Is.True);
        Assert.That(_producer.InError, Is.False);
    }

    [Test]
    public async Task ProducerSendMessage_ConsumerReadsMessage()
    {
        var testMessage = "Hello world";
        var mOut = new MemoryOutputStream();
        mOut.Write(testMessage);
        
        Assert.That(_producer.InError, Is.False);

        var sendRes = await _producer.Send(mOut.GetBuffer());
        
        Assert.That(sendRes, Is.True);
        Assert.That(_producer.InError, Is.False);
        
        var receiveRes = await _consumer.Receive();
        var mIn = new MemoryInputStream(receiveRes);
        var value = mIn.ReadString();
        
        Assert.That(value, Is.EqualTo(testMessage));
    }
}
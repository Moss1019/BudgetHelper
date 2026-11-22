using Messaging_lib.Stream;

namespace Messaging_libTest.Stream;

[TestFixture]
public class MemoryStreamTest
{
    [Test]
    public void TestStreams()
    {
        var sutOut = new MemoryOutputStream();
        var testValue = "Hello world";
        var now = DateTime.Now;
        sutOut.Write(testValue);
        sutOut.Write(1);
        sutOut.Write(10.28);
        sutOut.Write(now);

        var buffer = sutOut.GetBuffer();
        
        var sutIn = new MemoryInputStream(buffer);
        var strValue = sutIn.ReadString();
        var intValue = sutIn.ReadInt();
        var doubleVal = sutIn.ReadDouble();
        var dateVal = sutIn.ReadDateTime();
        
        Assert.That(strValue, Is.EqualTo(testValue));
        Assert.That(intValue, Is.EqualTo(1));
        Assert.That(doubleVal, Is.EqualTo(10.28));
        Assert.That(dateVal, Is.EqualTo(now));
    }
}
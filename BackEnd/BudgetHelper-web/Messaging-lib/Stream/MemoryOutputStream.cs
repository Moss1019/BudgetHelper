using System.Text;

namespace Messaging_lib.Stream;

public class MemoryOutputStream : IDisposable
{
    private readonly MemoryStream buffer = new MemoryStream();
    private int length = 0;

    public int Length => length;

    public byte[] GetBuffer()
    {
        return buffer.ToArray();
    }

    public void Clear()
    {
        buffer.SetLength(0);
        length = 0;
    }

    public void Write(byte[] data)
    {
        buffer.Write(data, 0, data.Length);
        length += data.Length;
    }

    public void Write(int data)
    {
        var b = BitConverter.GetBytes(data);
        buffer.Write(b, 0, b.Length);
        length += b.Length;
    }

    public void Write(long data)
    {
        var b = BitConverter.GetBytes(data);
        buffer.Write(b, 0, b.Length);
        length += b.Length;
    }

    public void Write(double data)
    {
        var b = BitConverter.GetBytes(data);
        buffer.Write(b, 0, b.Length);
        length += b.Length;
    }

    public void Write(bool data)
    {
        Write(data ? 1 : 0);
    }

    public void Write(string data)
    {
        if (data == null)
        {
            Write(0);
            return;
        }

        var asBytes = Encoding.UTF8.GetBytes(data);
        Write(asBytes.Length);
        Write(asBytes);
    }

    public void Write(DateTime time)
    {
        var asStr = time.ToString("o"); // ISO 8601 format
        Write(asStr);
    }

    public void Dispose()
    {
        buffer.Dispose();
    }
}

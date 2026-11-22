using System;
using System.IO;

namespace Messaging_lib.Stream;

public class MemoryInputStream : IDisposable
{
    private readonly int size;
    private readonly MemoryStream buffer;
    private int offset = 0;

    public MemoryInputStream(byte[] data)
    {
        buffer = new MemoryStream(data);
        size = data.Length;
    }

    public int Remaining => size - offset;

    public int ReadInt()
    {
        var b = new byte[sizeof(int)];
        ReadFull(b, 0, b.Length);
        offset += b.Length;
        return BitConverter.ToInt32(b, 0);
    }

    public long ReadLong()
    {
        var b = new byte[sizeof(long)];
        ReadFull(b, 0, b.Length);
        offset += b.Length;
        return BitConverter.ToInt64(b, 0);
    }

    public double ReadDouble()
    {
        var b = new byte[sizeof(double)];
        ReadFull(b, 0, b.Length);
        offset += b.Length;
        return BitConverter.ToDouble(b, 0);
    }

    public string ReadString()
    {
        var length = ReadInt();
        if (length == 0)
        {
            return "";
        }
        var bytes = new byte[length];
        ReadFull(bytes, 0, length);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    public bool ReadBoolean()
    {
        var value = ReadInt();
        return value == 1;
    }

    public byte[] ReadSome(int length)
    {
        var b = new byte[length];
        ReadFull(b, 0, length);
        offset += length;
        return b;
    }

    public DateTime ReadDateTime()
    {
        var asStr = ReadString();
        return DateTime.Parse(asStr);
    }

    private void ReadFull(byte[] buffer, int index, int count)
    {
        int totalRead = 0;
        while (totalRead < count)
        {
            int read = this.buffer.Read(buffer, index + totalRead, count - totalRead);
            if (read == 0)
            {
                throw new IOException("Unexpected end of stream");
            }
            totalRead += read;
        }
    }

    public void Dispose()
    {
        buffer.Dispose();
    }
}

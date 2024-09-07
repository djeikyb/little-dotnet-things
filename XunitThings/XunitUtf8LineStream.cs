using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Xunit.Abstractions;

namespace Merviche.XunitThings;

/// <summary>
/// Streams bytes to an <see cref="ITestOutputHelper"/>.
/// Assumes those bytes represent utf-8 strings.
/// Buffers until a newline is encountered.
/// Call <see cref="Flush"/> to <see cref="ITestOutputHelper.WriteLine(string)"/> whatever is in the buffer immediately.
/// </summary>
public class XunitUtf8LineStream : Stream
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MemoryStream _buffer;
    private readonly byte[] _newLine;

    public XunitUtf8LineStream(ITestOutputHelper testOutputHelper, string? newLine = null)
    {
        _testOutputHelper = testOutputHelper;
        _buffer = new MemoryStream();

        newLine ??= Environment.NewLine;
        _newLine = Encoding.UTF8.GetBytes(newLine);
        Debug.Assert(_newLine.Length is >= 1 and <= 2,
            "Look the options were \\n, \\r, or \\r\\n. May god have mercy on your soul.");
    }

    public override void Flush() => _testOutputHelper.WriteLine(Encoding.UTF8.GetString(_buffer.ToArray()));
    public override int Read(byte[] buffer, int offset, int count) => _buffer.Read(buffer, offset, count);
    public override long Seek(long offset, SeekOrigin origin) => _buffer.Seek(offset, origin);
    public override void SetLength(long value) => _buffer.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count)
    {
        var incoming = buffer.AsSpan(offset, count);
        if (TryFindNewLine(incoming, out var i))
        {
            _buffer.Write(incoming.Slice(0, i.Value));
            _testOutputHelper.WriteLine(Encoding.UTF8.GetString(_buffer.ToArray()));
            _buffer.SetLength(0);
            _buffer.Write(incoming.Slice(0, i.Value));
        }
        else
        {
            _buffer.Write(buffer, offset, count);
        }
    }

    private bool TryFindNewLine(Span<byte> haystack, [NotNullWhen(true)] out int? indexOfFirstNewLineByte)
    {
        indexOfFirstNewLineByte = haystack.IndexOf(_newLine[0]);
        if (indexOfFirstNewLineByte == -1)
        {
            indexOfFirstNewLineByte = null;
            return false;
        }

        if (_newLine.Length == 1) return true;

        if (haystack.Length < _newLine.Length)
        {
            indexOfFirstNewLineByte = null;
            return false;
        }

        if (haystack[indexOfFirstNewLineByte.Value + 1] != _newLine[1])
        {
            indexOfFirstNewLineByte = null;
            return false;
        }

        // no such thing as a newline that takes more than two bytes, right?
        return true;
    }

    public override long Length => _buffer.Length;
    public override bool CanRead => true;
    public override bool CanWrite => true;
    public override bool CanSeek => false;

    public override long Position
    {
        get => _buffer.Position;
        set => throw new NotSupportedException();
    }

    protected override void Dispose(bool disposing) => Flush();
}
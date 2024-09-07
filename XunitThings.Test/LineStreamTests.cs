using System.Diagnostics;
using System.Text;
using Merviche.XunitThings;
using Xunit.Abstractions;

namespace XunitThings.Test;

public class LineStreamTests(ITestOutputHelper testOutputHelper)
{
    private const int Delay = 250;

    [Fact]
    public async Task YourNewLineSequence()
    {
        var stream = new XunitUtf8LineStream(testOutputHelper);
        await Run(stream, Environment.NewLine);
    }

    [Fact]
    public async Task Posix()
    {
        var stream = new XunitUtf8LineStream(testOutputHelper, newLine: "\n");
        await Run(stream, nlsequence: "\n");
    }

    [Fact]
    public async Task Commodore64()
    {
        var stream = new XunitUtf8LineStream(testOutputHelper, newLine: "\r");
        await Run(stream, "\r");
    }

    [Fact]
    public async Task BbcComputerLiteracyProject()
    {
        var stream = new XunitUtf8LineStream(testOutputHelper, newLine: "\n\r");
        await Run(stream, "\n\r");
    }

    [Fact]
    public async Task IbmCompatiblePersonalComputers()
    {
        var stream = new XunitUtf8LineStream(testOutputHelper, newLine: "\r\n");
        await Run(stream, "\r\n");
    }

    private async Task Run(XunitUtf8LineStream stream, string nlsequence)
    {
        var sw = Stopwatch.StartNew();
        stream.Write("hello "u8);
        testOutputHelper.WriteLine($"{sw.ElapsedMilliseconds}] streamed 'hello '");
        Assert.Equal(expected: 6, actual: stream.Position);
        stream.Write("world"u8);
        testOutputHelper.WriteLine($"{sw.ElapsedMilliseconds}] streamed 'world'");
        Assert.Equal(expected: 11, actual: stream.Position);
        testOutputHelper.WriteLine($"{sw.ElapsedMilliseconds}] gonna wait for a newline");
        await Task.Delay(Delay);
        Assert.Equal(expected: 11, actual: stream.Position);
        stream.Write(Encoding.UTF8.GetBytes(nlsequence));
        testOutputHelper.WriteLine($"{sw.ElapsedMilliseconds}] sent newline!");
        Assert.Equal(expected: 0, actual: stream.Position);
        stream.Write("huh"u8);
        testOutputHelper.WriteLine($"{sw.ElapsedMilliseconds}] waiting for 'huh'");
        await Task.Delay(Delay);
        stream.Write(Encoding.UTF8.GetBytes(nlsequence));
        testOutputHelper.WriteLine($"{sw.ElapsedMilliseconds}] done!");
        stream.Write("one last thing"u8);
        stream.Flush();
    }
}
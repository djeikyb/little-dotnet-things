using System.Text;
using Xunit.Abstractions;

namespace Merviche.XunitThings;

/// Console.Out is a TextWriter.
/// Why isn't ITestOutputHelper?
/// https://gist.github.com/djeikyb/cf8f81e6917335b346534f5f072a1242
public class XunitTextWriter(ITestOutputHelper output) : TextWriter
{
    public override Encoding Encoding { get; } = Encoding.UTF8;
    public override void WriteLine(string? value) => output.WriteLine(value);
}
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace XunitThings;

/// A simpler hack around xunit's determined hostility:
/// https://gist.github.com/djeikyb/cf8f81e6917335b346534f5f072a1242
public class XunitTextWriter : TextWriter
{
    private readonly ITestOutputHelper _output;
    public XunitTextWriter(ITestOutputHelper output) => _output = output;
    public override Encoding Encoding { get; } = Encoding.UTF8;
    public override void WriteLine(string? value) => _output.WriteLine(value);
}

/// A barely functional logger to standard out for xunit.
/// https://gist.github.com/djeikyb/cf8f81e6917335b346534f5f072a1242
public class XunitLogger<T> : ILogger<T>
{
    private readonly ITestOutputHelper _outputHelper;

    public XunitLogger(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => throw new NotImplementedException();

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) =>
        _outputHelper.WriteLine(
            $"{logLevel.ToString()[..3]}] {formatter.Invoke(state, exception)}\n\n{exception}"
        );
}
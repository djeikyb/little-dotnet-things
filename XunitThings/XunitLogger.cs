using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Merviche.XunitThings;

/// A barely functional logger to standard out for xunit.
/// https://gist.github.com/djeikyb/cf8f81e6917335b346534f5f072a1242
public class XunitLogger<T>(ITestOutputHelper outputHelper) : ILogger<T>
{
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => NoopDisposable.Instance;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) =>
        outputHelper.WriteLine(
            $"{logLevel.ToString()[..3]}] {formatter.Invoke(state, exception)}\n\n{exception}"
        );
}
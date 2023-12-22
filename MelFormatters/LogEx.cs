using Microsoft.Extensions.Logging;

namespace MelFormatters;

public static class LogEx
{
    public static ScopeStateBuilder<T> With<T>(this ILogger<T> logger, string k, object? v) => new(logger, k, v);

    public sealed class ScopeStateBuilder<T> : ILogger<T>
    {
        private readonly ILogger<T> _logger;
        private readonly IDictionary<string, object?> _state = new Dictionary<string, object?>();

        public ScopeStateBuilder(ILogger<T> logger, string k, object? v)
        {
            _logger = logger;
            _state[k] = v;
        }

        public ScopeStateBuilder<T> With(string k, object? v)
        {
            _state[k] = v;
            return this;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter
        )
        {
            using var _ = _logger.BeginScope(_state);
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel) => _logger.IsEnabled(logLevel);
        public IDisposable? BeginScope() => _logger.BeginScope(_state);

        /// Why would you use this? Use the fluent <see cref="With"/> and <see cref="BeginScope()"/> instead!
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _logger.BeginScope(state);
    }
}

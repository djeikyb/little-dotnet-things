using Microsoft.Extensions.Logging;

namespace Merviche.Mel;

public static class LogEx
{
    /// <inheritdoc cref="ScopeStateBuilder{T}.With"/>
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

        /// <param name="k">
        /// <para>
        /// If using serilog, the value will be destructured (eg as
        /// simple json) when the key starts with an @.
        /// </para>
        /// <code>
        /// .With("@rate", rate); // {"rate": {"tier": "…", "qty": "…"}}
        /// </code>
        /// <para>
        /// vs
        /// </para>
        /// <code>
        /// _logger.With("rate_tier", rate.Tier).With("rate_qty", rate.Qty);
        /// </code>
        /// </param>
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
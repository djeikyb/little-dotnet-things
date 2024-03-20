using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace Merviche.MelFormatters;

// was https://gist.github.com/djeikyb/eeb600e1ce3619059adec6c8ca304923

public class HiCaMelFmattr : ConsoleFormatter
{
    public HiCaMelFmattr() : base("customName")
    {
    }

    public override void Write<TState>(
        in LogEntry<TState> le,
        IExternalScopeProvider? scopeProvider,
        TextWriter wr
    )
    {
        wr.Style();
        wr.WriteLine("---");

        wr.Write("level");
        wr.Write('=');
        wr.WriteLine(le.LogLevel);

        wr.Write("category");
        wr.Write('=');
        wr.WriteLine(le.Category);

        if (le.EventId != default)
        {
            wr.Write("eventid");
            wr.Write('=');
            wr.WriteLine(le.EventId);
        }

        scopeProvider?.ForEachScope(
            (scope, state) =>
            {
                if (scope is IEnumerable<KeyValuePair<string, object?>> kvPairs)
                {
                    foreach (var kv in kvPairs)
                    {
                        state.Style(text: 3);
                        state.Write(kv.Key);
                        state.Write('=');
                        state.Style();

                        if (kv.Value is null)
                        {
                            state.Style(93,1,5);
                            state.WriteLine("null");
                            state.Style();
                        }
                        else
                        {
                            state.WriteLine(kv.Value);
                        }

                        state.Style();
                    }
                }
                else // not kv pairs? but why?
                {
                    var s = Convert.ToString(scope, CultureInfo.InvariantCulture);
                    if (s == null)
                    {
                        state.Style(fg: 93, text: 1);
                        state.WriteLine("null");
                        state.Style();
                    }
                    else
                    {
                        state.Style(fg: 96);
                        state.WriteLine(s);
                        state.Style();
                    }
                }
            },
            wr
        );
        string msg = le.Formatter(le.State, le.Exception);
        wr.WriteLine(msg);
        if (le.Exception is not null)
        {
            wr.WriteLine(le.Exception.ToString());
        }
    }
}
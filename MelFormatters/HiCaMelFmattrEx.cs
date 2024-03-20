using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Merviche.MelFormatters;

public static class HiCaMelFmattrEx
{
    public static ILoggingBuilder AddHiCaMelFmattr(
        this ILoggingBuilder lb,
        Action<ConsoleFormatterOptions>? configure = null
    )
    {
        return lb.AddConsole(o => o.FormatterName = "customName")
            .AddConsoleFormatter<HiCaMelFmattr, ConsoleFormatterOptions>(configure ?? (_ => { }));
    }

}
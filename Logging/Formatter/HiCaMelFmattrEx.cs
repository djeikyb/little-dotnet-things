using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Merviche.Logging.Formatter;

public static class HiCaMelFmattrEx
{
    public static ILoggingBuilder AddHiCaMelFmattr(this ILoggingBuilder lb)
    {
        var ll = lb.AddConsole(o => o.FormatterName = typeof(HiCaMelFmattr).FullName);
        lb.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ConsoleFormatter, HiCaMelFmattr>());
        return ll;
    }
}
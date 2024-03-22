using Merviche.Logging;
using Merviche.Logging.Formatter;
using Microsoft.Extensions.Logging;

var lf = LoggerFactory
    .Create(lb => lb.AddHiCaMelFmattr());
// .Create(lb => lb.AddJsonConsole(o => o.IncludeScopes = true));
var logger = lf.CreateLogger<Program>();
using var _ = logger.With("app", "example").BeginScope();
var order = new Order { Quantity = 3 };
logger.With("hey", "ho")
    .Value(order.Quantity)
    .With("oops", null)
    .With("🐛", "🐞")
    .LogInformation("pbbbtth");
logger.LogInformation("fwop");
try
{
    throw new Exception("but..");
}
catch (Exception e)
{
    logger.LogError(e, "no!");
}

class Order
{
    public required int Quantity { get; init; }
}
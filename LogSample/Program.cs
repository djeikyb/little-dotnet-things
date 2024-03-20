// See https://aka.ms/new-console-template for more information

using Merviche.Logging;
using Merviche.Logging.Formatter;
using Microsoft.Extensions.Logging;

var lf = LoggerFactory
    .Create(lb => lb.AddHiCaMelFmattr());
// .Create(lb => lb.AddJsonConsole(o => o.IncludeScopes = true));
var mel = lf.CreateLogger<Program>();
using var _ = mel.With("app", "example").BeginScope();
mel.With("hey", "ho")
    .With("oops", null)
    .With("🐛", "🐞")
    .LogInformation("pbbbtth");
mel.LogInformation("fwop");
try
{
    throw new Exception("but..");
}
catch (Exception e)
{
    mel.LogError(e, "no!");
}
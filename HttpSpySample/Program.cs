// See https://aka.ms/new-console-template for more information

using Merviche.HttpSpy;
using Merviche.MelFormatters;
using Microsoft.Extensions.Logging;

var lf = LoggerFactory
    .Create(lb => lb.AddHiCaMelFmattr().AddFilter("HttpSpy", LogLevel.Trace));
var httpClient = new HttpClient(new HttpSpyHandler(lf.CreateLogger<HttpSpyHandler>(), new HttpClientHandler()));
var rs = await httpClient.GetAsync("http://google.com");
lf.CreateLogger<Program>()
    .With("http.rs.status", rs.StatusCode)
    .With("http.rs.content-length", rs.Content.Headers.ContentLength)
    .LogInformation("Done.");
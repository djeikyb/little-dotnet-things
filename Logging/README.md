Try this:

```csharp
logger.With("just_one_key", "for one log event 🐙")
      .LogInformation("🪼");
```

instead of:

```csharp
using (logger.BeginScope(new Dictionary<string, object?>
{
   { "for_just_one_log_event", "this is a lot of code 🐛" },
    // another key could go on this line
}))
{
    logger.LogInformation("so much boilerplate");
}
```

Or:

```csharp
using var _ = logger
    .With("correlation_id", "some value")
    .With("username", "some value")
    .With("ip_address", "some value")
    .BeginScope();
logger.LogInformation("isn't this a bit nicer?");
```

instead of:

```csharp
var state = new Dictionary<string, object?>();
state["correlation_id"] = "some value";
state["username"] = "some value";
state["ip_address"] = "some value";
using var _ = logger.BeginScope(state);
logger.LogInformation("isn't this kinda awkward");
```
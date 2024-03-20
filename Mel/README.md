Try this:

```csharp
logger
    .With("just_one_key", "🐙")
    .LogInformation("🪼");
```

or

```csharp
using var _ = logger
    .With("correlation_id", "some value")
    .With("username", "some value")
    .With("ip_address", "some value")
    .BeginScope();
logger.LogInformation("isn't this a bit nicer?");
```

Instead of:

```csharp
var state = new Dictionary<string, object?>();
state["correlation_id"] = "some value";
state["username"] = "some value";
state["ip_address"] = "some value";
using var _ = logger.BeginScope(state);
logger.LogInformation("this is awkward");
```
Try this:

```csharp
logger.With("just_one_key", "🐙")
      .LogInformation("🪼");
```

instead of:

```csharp
var state = new Dictionary<string, object?>();
state["just_one_key"] = "is a lot of work 🐛";
using var _ = logger.BeginScope(state);
logger.LogInformation("so much boilerplate");
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
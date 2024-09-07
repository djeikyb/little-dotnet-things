You have a class that writes to standard out.

```csharp
public class SomeService
{
    private readonly TextWriter _stdout;

    public SomeService(TextWriter? stdout = null)
    {
        _stdout = stdout ?? Console.Out;
    }

    public void DoSomething()
    {
        _stdout.WriteLine("Gonna do it!");
        // ...
    }
}
```

But all you have is an ITestOutputHelper, so no lines are printed.

Until now:

```csharp
public class YourTestClass(ITestOutputHelper helper)
{
    [Fact]
    public void TestName() => new SomeService(new XunitTextWriter(helper)).DoSomething();
}
```

* * *

You have a class with a logger.

```csharp
public class SomeService
{
    private readonly ILogger<SomeService> _logger;
    public SomeService(ILogger<SomeService> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation("Gonna do it!");
        // ...
    }
}
```

But all you have is an ITestOutputHelper, so no lines are logged.

Until now:

```csharp
public class YourTestClass(ITestOutputHelper helper)
{
    [Fact]
    public void TestName() => new SomeService(new XunitLogger(helper)).DoSomething();
}
```
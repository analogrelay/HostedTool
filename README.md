# Microsoft.Extensions.Hosting + System.CommandLine = 😁

## Example

```csharp
internal class Program
{
    private readonly ILogger<Program> _logger;

    internal static Task Main(string[] args) => CreateHostBuilder(args).RunToolAsync();

    internal static IHostBuilder CreateHostBuilder(string[] args)
    {
        return ToolHost.CreateDefaultBuilder(args);
    }

    public Program(ILogger<Program> logger)
    {
        _logger = logger;
    }

    public void ExecuteAsync(string simpleOption, bool boolOption, int? optionalInt, IEnumerable<string> arguments)
    {
        _logger.LogInformation("simpleOption: {Value}", simpleOption);
        _logger.LogInformation("boolOption: {Value}", boolOption);
        _logger.LogInformation("optionalInt: {Value}", optionalInt);
        _logger.LogInformation("arguments: {Value}", string.Join(",", arguments));
    }
}
```

```
> dotnet run --project .\samples\sample-tool\ -- --simple-option foo a b c d
info: SampleTool.Program[0]
      simpleOption: foo
info: SampleTool.Program[0]
      boolOption: False
info: SampleTool.Program[0]
      optionalInt: (null)
info: SampleTool.Program[0]
      arguments: a,b,c,d
```
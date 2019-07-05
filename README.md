# Microsoft.Extensions.Hosting + System.CommandLine = 😁

## Example

```csharp
internal class Program
{
    private readonly ILogger<Program> _logger;

    internal static Task Main(string[] args) => ToolHost.CreateDefaultBuilder(args).RunToolAsync();

    public Program(ILogger<Program> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Does simple things.
    /// </summary>
    /// <param name="simpleOption">A simple string option.</param>
    /// <param name="boolOption">A boolean option.</param>
    /// <param name="optionalInt">An optional integer.</param>
    /// <param name="arguments">The other arguments.</param>
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
> dotnet run --project .\samples\sample-tool\ -- --help
sample-tool:
  Does simple things.

Usage:
  sample-tool [options] [<arguments>...]

Arguments:
  <arguments>    The other arguments.

Options:
  --simple-option <simple-option>    A simple string option.
  --bool-option                      A boolean option.
  --optional-int <optional-int>      An optional integer.
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
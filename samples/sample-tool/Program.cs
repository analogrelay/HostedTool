using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contrib.Extensions.Hosting.Tool;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleTool
{
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
}

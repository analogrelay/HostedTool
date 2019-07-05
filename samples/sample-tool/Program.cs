using System.Collections.Generic;
using System.Threading.Tasks;
using Contrib.Extensions.Hosting.Tool;
using Microsoft.Extensions.Logging;

namespace SampleTool
{
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
}

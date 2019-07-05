using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Contrib.Extensions.Hosting.Tool
{
    internal class DefaultEntryPoint : IEntryPoint
    {
        private readonly ICommandLineAccessor _commandLineAccessor;
        private readonly ICommandDiscoverer _discoverer;
        private readonly ILogger<DefaultEntryPoint> _logger;

        public DefaultEntryPoint(ICommandLineAccessor commandLineAccessor, ICommandDiscoverer discoverer, ILogger<DefaultEntryPoint> logger)
        {
            _commandLineAccessor = commandLineAccessor;
            _discoverer = discoverer;
            _logger = logger;
        }

        public Task<int> ExecuteAsync()
        {
            // Discover commands
            var builder = _discoverer.CreateCommandLineBuilder();

            builder.UseDebugDirective();
            builder.UseHelp();

            var parser = builder.Build();

            var parsed = parser.Parse(_commandLineAccessor.Arguments);

            return parser.InvokeAsync(_commandLineAccessor.Arguments.ToArray());
        }
    }
}
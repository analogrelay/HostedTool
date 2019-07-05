using System.CommandLine.Invocation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Contrib.Extensions.Hosting.Tool
{
    internal class DefaultEntryPoint : IEntryPoint
    {
        private readonly ICommandLineAccessor _commandLineAccessor;
        private readonly ICommandDiscoverer _discoverer;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly ILogger<DefaultEntryPoint> _logger;

        public DefaultEntryPoint(ICommandLineAccessor commandLineAccessor, ICommandDiscoverer discoverer, IHostApplicationLifetime applicationLifetime, ILogger<DefaultEntryPoint> logger)
        {
            _commandLineAccessor = commandLineAccessor;
            _discoverer = discoverer;
            _applicationLifetime = applicationLifetime;
            _logger = logger;
        }

        public Task<int> ExecuteAsync()
        {
            // Discover commands
            var builder = _discoverer.CreateCommandLineBuilder();

            builder.UseMiddleware((context, next) =>
            {
                context.BindingContext.AddService(typeof(CancellationToken), () => _applicationLifetime.ApplicationStopping);
                return next(context);
            });
            builder.RegisterWithDotnetSuggest();
            builder.UseSuggestDirective();
            builder.UseExceptionHandler((ex, context) =>
            {
                context.InvocationResult = new ErrorResult(ex, _logger);
            });
            builder.UseDebugDirective();
            builder.UseHelp();

            var parser = builder.Build();

            return parser.InvokeAsync(_commandLineAccessor.Arguments.ToArray());
        }
    }
}
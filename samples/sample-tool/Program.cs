using System;
using System.Threading;
using System.Threading.Tasks;
using Contrib.Extensions.Hosting.Tool;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleTool
{
    internal class Program
    {
        private readonly ILogger<Program> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        internal static Task<int> Main(string[] args) => ToolHost.CreateDefaultBuilder(args).RunToolAsync();

        public Program(ILogger<Program> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Does simple things.
        /// </summary>
        /// <param name="message">A message to print.</param>
        /// <param name="countTo">Counts up to this value, once per second.</param>
        /// <param name="unexpectedError">If present, throws an unexpected exception.</param>
        /// <param name="expectedError">If present, throws an expected exception.</param>
        /// <param name="exitCode">The exit code to return.</param>
        /// <param name="cancellationToken">Triggered when Ctrl-C is pressed.</param>
        public async Task<int> ExecuteAsync(string message, int countTo, bool unexpectedError, bool expectedError, int? exitCode, CancellationToken cancellationToken)
        {
            try
            {
                if (unexpectedError)
                {
                    throw new Exception("KABOOM!");
                }

                if (expectedError)
                {
                    throw new CommandLineException("KABOOM!", exitCode: 42);
                }

                _logger.LogInformation("App Name: {ContentRootPath}", _hostEnvironment.ApplicationName);
                _logger.LogInformation("Content root: {ContentRootPath}", _hostEnvironment.ContentRootPath);

                _logger.LogInformation("Message: {Message}", message);

                for (var i = 0; i < countTo; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    _logger.LogInformation("Count: {Count}", i + 1);
                    await Task.Delay(1000, cancellationToken);
                }

                return exitCode ?? 0;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Graceful shutdown!");
                return 0;
            }
        }
    }
}

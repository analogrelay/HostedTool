using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.DragonFruit;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Contrib.Extensions.Hosting.Tool
{
    internal class DefaultCommandDiscoverer : ICommandDiscoverer
    {
        private readonly IServiceProvider _services;

        public DefaultCommandDiscoverer(IServiceProvider services)
        {
            _services = services;
        }

        public CommandLineBuilder CreateCommandLineBuilder()
        {
            // Check for the original EntryPoint
            var exeAsm = Assembly.GetEntryAssembly();
            var entryPoint = exeAsm.EntryPoint;
            if (entryPoint == null)
            {
                throw new InvalidOperationException("Failed to locate assembly entry point!");
            }
            var programType = entryPoint.DeclaringType;

            var executeMethods = programType
                .GetMethods()
                .Where(m => m.IsPublic && m.Name.Equals("ExecuteAsync", StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (executeMethods.Count == 0)
            {
                throw new InvalidOperationException($"Unable to find a '{programType.FullName}.ExecuteAsync' method!");
            }
            else if (executeMethods.Count > 1)
            {
                throw new AmbiguousMatchException($"Multiple '{programType.FullName}.ExecuteAsync' methods were found!");
            }

            var executeMethod = executeMethods[0];

            var builder = new CommandLineBuilder();

            // The 'target' argument isn't even used...
            builder.Command.ConfigureFromMethod(executeMethod, target: () => null);

            // Try to add docs from XML doc comments
            builder.ConfigureHelpFromXmlComments(executeMethod, Path.ChangeExtension(exeAsm.Location, ".xml"));

            // Activate the target via DI
            builder.UseMiddleware(c => c.BindingContext.AddService(executeMethod.DeclaringType, () => ActivatorUtilities.CreateInstance(_services, executeMethod.DeclaringType, Array.Empty<object>())));

            return builder;
        }
    }
}
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;

namespace Contrib.Extensions.Hosting.Tool
{
    public interface ICommandDiscoverer
    {
        CommandLineBuilder CreateCommandLineBuilder();
    }
}

using System;
using Microsoft.Extensions.CommandLineUtils;

namespace DemoNancy
{
    internal class CommandLineArgsConfig
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        public CommandLineArgsConfig(string defaultHost, int defaultPort)
        {
            Host = defaultHost;
            Port = defaultPort;
        }

        public void SetConfigFromArgs(string[] args)
        {
            // https://blog.terribledev.io/Parsing-cli-arguments-in-dotnet-core-Console-App/
            // https://msdn.microsoft.com/en-us/magazine/mt763239.aspx
            // http://jameschambers.com/2015/09/supporting-options-and-arguments-in-your-dnx-commands/
            // https://docs.microsoft.com/en-us/aspnet/core/api/microsoft.extensions.commandlineutils.commandlineapplication
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);
            app.Command("host", config =>
            {
                config.Description = "Set host";
                var arg = config.Argument("host", "host site from here", false);
                config.OnExecute(() => {
                    if (!string.IsNullOrWhiteSpace(arg.Value)) {
                        Host = arg.Value;
                        return 0;
                    }
                    return 1;
                });
            });
            app.Command("port", config =>
            {
                config.Description = "Set port number";
                var arg = config.Argument("port", "host from this port", false);
                config.OnExecute(() =>
                {
                    int port = 0;
                    if (Int32.TryParse(arg.Value, out port)) {
                        Port = port;
                        return 0;
                    }
                    return 1;
                });
            });
            app.HelpOption("-? | -h | --help");
            app.Execute(args);
        }
    }
}

using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DemoNancy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new CommandLineArgsConfig("*", 5000);
            config.SetConfigFromArgs(args);

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://{config.Host}:{config.Port}")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}

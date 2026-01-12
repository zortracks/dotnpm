using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotNpm.Tests.Worker {

    public class Program {

        public static void Main(string[] args) {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDotNpm("local", builder => {
                builder.WithLocalPackage();
            });
            builder.Services.AddDotNpm("built-in", builder => {
                builder.WithBuiltInPackage("built-in", builder => {
                    builder.WithVersion("1.0.0");
                });
            });
            builder.Services.AddHostedService<Worker>();

            builder.Build().Run();
        }
    }
}
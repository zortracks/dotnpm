using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotNpm.Tests.Worker {

    public class Program {

        public static void Main(string[] args) {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDotNpm("built-in", builder => {
                var inputCssFile = null as BuiltInCssSourceFile;

                builder.WithSources(builder => {
                    builder.AddCssFile("input.css", builder => {
                        builder.WithImport("tailwindcss");
                    }, out inputCssFile);
                });

                builder.WithAssetFile("./dist/output.css", builder => {

                });

                builder.WithPackage(builder => {
                    builder.WithDevDependency("tailwindcss", builder => builder.WithLatestVersion());
                    builder.WithDevDependency("@tailwindcss/postcss", builder => builder.WithLatestVersion());
                    builder.WithDevDependency("postcss", builder => builder.WithLatestVersion());

                    builder.WithScript("dev", builder => {
                        //builder.Inline("npx tailwindcss -i ./src/input.css -o ./dist/output.css --watch");
                    });
                });
            });
            builder.Services.AddHostedService<Worker>();

            builder.Build().Run();
        }
    }
}
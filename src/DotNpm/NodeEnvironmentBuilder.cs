using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Json;

namespace DotNpm {

    public sealed class NodeEnvironmentBuilder {
        private readonly string _environmentName;
        private readonly IServiceProvider _serviceProvider;

        public NodeEnvironmentBuilder(IServiceProvider serviceProvider, string environmentName) {
            _serviceProvider = serviceProvider;
            _environmentName = environmentName;

            Environment = ActivatorUtilities.CreateInstance<NodeEnvironment>(_serviceProvider);
            Environment.Name = _environmentName;
            Environment.Directory = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, _environmentName));
        }

        public NodeEnvironment Environment { get; }

        public NodeEnvironmentBuilder WithBuiltInPackage(string name, Action<PackageBuilder> builder) {
            var packageBuilder = new PackageBuilder(_serviceProvider, name);

            builder.Invoke(packageBuilder);

            return WithPackage(packageBuilder.Package);
        }

        public NodeEnvironmentBuilder WithDirectory(string directory) {
            if (Path.IsPathRooted(directory))
                return WithDirectory(new DirectoryInfo(directory));
            else
                return WithDirectory(new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, directory)));
        }

        public NodeEnvironmentBuilder WithDirectory(DirectoryInfo directory) {
            Environment.Directory = directory;

            return this;
        }

        public NodeEnvironmentBuilder WithLocalPackage(string fileName = "package.json") {
            var jsonSerializerOptions = new JsonSerializerOptions();

            jsonSerializerOptions.Converters.Add(new LocalPackage.LocalPackageConverter(_serviceProvider));
            jsonSerializerOptions.Converters.Add(new Dependency.DependencyConverter());
            jsonSerializerOptions.Converters.Add(new LocalScript.LocalScriptConverter(_serviceProvider));

            return WithPackage(JsonSerializer.Deserialize<LocalPackage>(File.ReadAllText(Path.Combine(Environment.Directory.FullName, fileName)), jsonSerializerOptions));
        }

        public NodeEnvironmentBuilder WithPackage(IPackage package) {
            Environment.Package = package;

            return this;
        }

        public NodeEnvironmentBuilder WithSources(Action<SourcesBuilder> builder) {
            var sourceBuilder = new SourcesBuilder(_serviceProvider, Environment.Directory);

            builder.Invoke(sourceBuilder);
            Environment.Source = sourceBuilder.Sources;

            return this;
        }
    }
}
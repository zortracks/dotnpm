using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DotNpm {

    public sealed class NodeEnvironmentBuilder {
        private readonly string _environmentName;

        private readonly IServiceCollection _services;

        public NodeEnvironmentBuilder(IServiceCollection services, string environmentName) {
            _services = services;
            _environmentName = environmentName;

            Directory = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, _environmentName));
        }

        public HashSet<Func<IServiceProvider, INodeAsset>> AssetsFactory { get; } = new HashSet<Func<IServiceProvider, INodeAsset>>();
        public DirectoryInfo Directory { get; private set; }
        public Func<IServiceProvider, IPackage> PackageFactory { get; private set; }
        public Func<IServiceProvider, Sources> SourcesFactory { get; private set; }

        public NodeEnvironment GetEnvironment(IServiceProvider serviceProvider) {
            var nodeEnvironment = ActivatorUtilities.CreateInstance<NodeEnvironment>(serviceProvider);

            nodeEnvironment.Name = _environmentName;
            nodeEnvironment.Directory = Directory;
            nodeEnvironment.Package = PackageFactory.Invoke(serviceProvider);
            nodeEnvironment.Sources = SourcesFactory.Invoke(serviceProvider);

            return nodeEnvironment;
        }

        public NodeEnvironmentBuilder WithAsset<TOutputFile>(IOutputFileReference<TOutputFile> outputReference, IScriptInvocationReference scriptInvocationReference) where TOutputFile : OutputFileBase {
            AssetsFactory.Add(serviceProvider => ActivatorUtilities.CreateInstance<NodeAsset<TOutputFile>>(serviceProvider, outputReference, scriptInvocationReference));

            return this;
        }

        public NodeEnvironmentBuilder WithBuiltInPackage(string name, Action<PackageBuilder> builder) {
            var packageBuilder = new PackageBuilder(_services, name);

            builder.Invoke(packageBuilder);
            PackageFactory = serviceProvider => packageBuilder.GetPackage(serviceProvider);

            return this;
        }

        public NodeEnvironmentBuilder WithDirectory(string directory) {
            if (Path.IsPathRooted(directory))
                return WithDirectory(new DirectoryInfo(directory));
            else
                return WithDirectory(new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, directory)));
        }

        public NodeEnvironmentBuilder WithDirectory(DirectoryInfo directory) {
            Directory = directory;

            return this;
        }

        public NodeEnvironmentBuilder WithLocalPackage(string fileName = "package.json") {
            PackageFactory = serviceProvider => {
                var jsonSerializerOptions = new JsonSerializerOptions();

                jsonSerializerOptions.Converters.Add(new LocalPackage.LocalPackageConverter(serviceProvider));
                jsonSerializerOptions.Converters.Add(new Dependency.DependencyConverter());
                jsonSerializerOptions.Converters.Add(new LocalScript.LocalScriptConverter(serviceProvider));

                return JsonSerializer.Deserialize<LocalPackage>(File.ReadAllText(Path.Combine(Directory.FullName, fileName)), jsonSerializerOptions);
            };

            return this;
        }

        public NodeEnvironmentBuilder WithPackage(IPackage package) {
            PackageFactory = _ => package;

            return this;
        }

        public NodeEnvironmentBuilder WithSources(Action<SourcesBuilder> builder) {
            var sourceBuilder = new SourcesBuilder(_services, Directory);

            builder.Invoke(sourceBuilder);
            SourcesFactory = serviceProvider => sourceBuilder.GetSources(serviceProvider);

            return this;
        }
    }
}
using DotNpm.Contexts;
using DotNpm.Elements;
using DotNpm.Internals;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNpm.Builders {

    public sealed class NodeEnvironmentBuilder {
        private readonly string _environmentName;
        private readonly string _packageName;
        private readonly IServiceCollection _services;
        private DirectoryInfo _workingDirectory;

        public NodeEnvironmentBuilder(IServiceCollection services, string environmentName) {
            _services = services;
            _environmentName = Path.Combine("/", environmentName);
            _packageName = Path.Combine(_environmentName, "package.json");

            _services.Configure<NodeEnvironment>(_environmentName, nodeEnvironment => {
                nodeEnvironment.Name = _environmentName;
            });
            _services.Configure<NodeEnvironmentContext>(_environmentName, context => {
                context.WorkingDirectory = _workingDirectory = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, _environmentName));
            });
        }

        public NodeEnvironmentBuilder WithAssetFile(string fileName, Action<AssetFileBuilder> builder) {
            builder.Invoke(new AssetFileBuilder(_services, _packageName, fileName));
            _services.Configure<Package>(_packageName, package => ((HashSet<string>)package.Files).Add(Path.GetRelativePath(_workingDirectory.FullName, fileName)));
            _services.Configure<NodeEnvironment>(_environmentName, nodeEnvironment => ((HashSet<string>)nodeEnvironment.Assets).Add(fileName));

            return this;
        }

        public NodeEnvironmentBuilder WithPackage(Action<PackageBuilder> builder) {
            builder.Invoke(new PackageBuilder(_services, _workingDirectory, _packageName));
            _services.Configure<NodeEnvironmentContext>(_environmentName, context => context.PackageName = _packageName);

            return this;
        }

        public NodeEnvironmentBuilder WithSourceFile(string fileName, Action<SourceFileBuilder> builder) {
            return this;
        }

        public NodeEnvironmentBuilder WithWorkingDirectory(string workingDirectory) => WithWorkingDirectory(workingDirectory.ToAbsoluteDirectory());

        public NodeEnvironmentBuilder WithWorkingDirectory(DirectoryInfo directory) {
            _services.Configure<NodeEnvironmentContext>(_environmentName, context => context.WorkingDirectory = directory);

            return this;
        }
    }
}
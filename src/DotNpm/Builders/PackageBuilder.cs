using DotNpm.Contexts;
using DotNpm.Elements;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNpm.Builders {

    public sealed class PackageBuilder {
        private readonly string _packageName;
        private readonly IServiceCollection _services;

        public PackageBuilder(IServiceCollection services, DirectoryInfo workingDirectory, string packageName) {
            _services = services;
            _packageName = packageName;

            _services.Configure<Package>(packageName, package => {
                package.Name = new FileInfo(_packageName).Name;
            });
            _services.Configure<PackageContext>(context => {
                context.WorkingDirectory = workingDirectory;
            });
        }

        public PackageBuilder WithDependency(string dependencyName, string dependency, string version = "latest") {
            _services.Configure<Package>(_packageName, package => ((Dictionary<string, string>)package.Dependencies).Add(dependency, version));

            return this;
        }

        public PackageBuilder WithDevDependency(string dependencyName, string dependency, string version = "latest") {
            _services.Configure<Package>(_packageName, package => ((Dictionary<string, string>)package.DevDependencies).Add(dependency, version));

            return this;
        }

        public PackageBuilder WithScript(string scriptName, Action<ScriptBuilder> builder) {
            builder.Invoke(new ScriptBuilder(_services, Path.Combine(_packageName, scriptName)));
            _services.Configure<Package>(_packageName, package => ((HashSet<string>)package.Scripts).Add(scriptName));

            return this;
        }
    }
}
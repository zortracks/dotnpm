using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNpm {

    public sealed class PackageBuilder {
        private readonly string _packageName;
        private readonly IServiceCollection _services;

        public PackageBuilder(IServiceCollection services, string packageName) {
            _services = services;
            _packageName = packageName;
        }

        public HashSet<Func<IServiceProvider, Dependency>> DependenciesFactory { get; } = new HashSet<Func<IServiceProvider, Dependency>>();
        public HashSet<Func<IServiceProvider, Dependency>> DevDependenciesFactory { get; } = new HashSet<Func<IServiceProvider, Dependency>>();
        public Func<IServiceProvider, Dist> DistFactory { get; private set; }
        public HashSet<Func<IServiceProvider, BuiltInScript>> ScriptsFactory { get; } = new HashSet<Func<IServiceProvider, BuiltInScript>>();
        public Func<string> VersionFactory { get; private set; }

        public BuiltInPackage GetPackage(IServiceProvider serviceProvider) {
            var builtInPackage = ActivatorUtilities.CreateInstance<BuiltInPackage>(serviceProvider);

            builtInPackage.Dependencies = DependenciesFactory.Select(dependencyFactory => dependencyFactory.Invoke(serviceProvider));
            builtInPackage.DevDependencies = DevDependenciesFactory.Select(dependencyFactory => dependencyFactory.Invoke(serviceProvider));
            builtInPackage.Dist = DistFactory?.Invoke(serviceProvider);
            builtInPackage.Name = _packageName;
            builtInPackage.Scripts = ScriptsFactory.Select(scriptFactory => scriptFactory.Invoke(serviceProvider));
            builtInPackage.Version = VersionFactory?.Invoke();

            return builtInPackage;
        }

        public PackageBuilder WithDependency(string dependencyName, Action<DependencyBuilder> builder) => WithDependency(DependenciesFactory, dependencyName, builder);

        public PackageBuilder WithDevDependency(string dependencyName, Action<DependencyBuilder> builder) => WithDependency(DevDependenciesFactory, dependencyName, builder);

        public PackageBuilder WithDist(Action<DistBuilder> builder) {
            var distBuilder = new DistBuilder(_services);

            builder.Invoke(distBuilder);
            DistFactory = serviceProvider => distBuilder.GetDist(serviceProvider);

            return this;
        }

        public PackageBuilder WithScript(string scriptName, Action<ScriptBuilder> builder, out IScriptInvocationReference scriptInvocationReference) {
            var scriptBuilder = new ScriptBuilder(_services, scriptName);

            builder.Invoke(scriptBuilder);
            ScriptsFactory.Add(serviceProvider => scriptBuilder.GetScript(serviceProvider));
            scriptInvocationReference = scriptBuilder.GetScriptInvocationReference();

            return this;
        }

        public PackageBuilder WithVersion(string version) {
            VersionFactory = () => version;

            return this;
        }

        private PackageBuilder WithDependency(HashSet<Func<IServiceProvider, Dependency>> target, string dependencyName, Action<DependencyBuilder> builder) {
            var dependencyBuilder = new DependencyBuilder(dependencyName);

            builder.Invoke(dependencyBuilder);
            target.Add(serviceProvider => dependencyBuilder.GetDependency(serviceProvider));

            return this;
        }
    }
}
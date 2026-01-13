using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DotNpm {

    public sealed class PackageBuilder {
        private readonly string _packageName;
        private readonly IServiceProvider _serviceProvider;

        public PackageBuilder(IServiceProvider serviceProvider, string packageName) {
            _serviceProvider = serviceProvider;
            _packageName = packageName;

            Package = ActivatorUtilities.CreateInstance<BuiltInPackage>(_serviceProvider);
            Package.Name = _packageName;
        }

        public BuiltInPackage Package { get; }

        public PackageBuilder WithDependency(string dependencyName, Action<DependencyBuilder> builder) {
            ((HashSet<Dependency>)Package.Dependencies).Add(GetDependency(dependencyName, builder));

            return this;
        }

        public PackageBuilder WithDevDependency(string dependencyName, Action<DependencyBuilder> builder) {
            ((HashSet<Dependency>)Package.DevDependencies).Add(GetDependency(dependencyName, builder));

            return this;
        }

        public PackageBuilder WithScript(string scriptName, Action<ScriptBuilder> builder) {
            var scriptBuilder = new ScriptBuilder(_serviceProvider, scriptName);

            builder.Invoke(scriptBuilder);
            ((HashSet<ScriptBase>)Package.Scripts).Add(scriptBuilder.Script);

            return this;
        }

        public PackageBuilder WithVersion(string version) {
            Package.Version = version;

            return this;
        }

        private Dependency GetDependency(string dependencyName, Action<DependencyBuilder> builder) {
            var dependencyBuilder = new DependencyBuilder(dependencyName);

            builder.Invoke(dependencyBuilder);

            return dependencyBuilder.Dependency ?? throw new InvalidOperationException("A version must be specified for dependency");
        }
    }
}
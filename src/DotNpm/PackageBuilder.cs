using Microsoft.Extensions.DependencyInjection;
using System;

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

        public PackageBuilder WithVersion(string version) {
            Package.Version = version;

            return this;
        }
    }
}
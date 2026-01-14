using System;

namespace DotNpm {

    public sealed class DependencyBuilder {
        private readonly string _dependencyName;

        public DependencyBuilder(string dependencyName) {
            this._dependencyName = dependencyName;
        }

        public Func<IServiceProvider, string> Version { get; private set; }

        public Dependency GetDependency(IServiceProvider serviceProvider) => new Dependency(_dependencyName) {
            Version = Version.Invoke(serviceProvider)
        };

        public void WithLatestVersion() => WithVersion("latest");

        public void WithVersion(string version) => Version = _ => version;
    }
}
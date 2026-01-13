namespace DotNpm {

    public sealed class DependencyBuilder {

        public DependencyBuilder(string dependencyName) {
            Dependency = new Dependency(dependencyName);
        }

        public Dependency Dependency { get; private set; }

        public void WithLatestVersion() => Dependency.Version = "latest";

        public void WithVersion(string version) => Dependency.Version = version;
    }
}
using System.Collections.Generic;

namespace DotNpm.Elements {

    public sealed class Package {
        public IReadOnlyDictionary<string, string> Dependencies { get; } = new Dictionary<string, string>();
        public IReadOnlyDictionary<string, string> DevDependencies { get; } = new Dictionary<string, string>();
        public IReadOnlySet<string> Files { get; } = new HashSet<string>();
        public string Name { get; internal set; }
        public IReadOnlySet<string> Scripts { get; } = new HashSet<string>();
        public string Version { get; internal set; } = "0.0.0";
    }
}
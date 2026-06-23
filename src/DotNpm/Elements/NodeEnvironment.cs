using System.Collections.Generic;

namespace DotNpm.Elements {

    public sealed class NodeEnvironment {
        public IReadOnlySet<string> Assets { get; } = new HashSet<string>();
        public string Name { get; internal set; }
    }
}
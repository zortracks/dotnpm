using System.Collections.Generic;

namespace DotNpm {

    public sealed class Sources {
        public HashSet<SourceFileBase> Files { get; } = new HashSet<SourceFileBase>();
    }
}
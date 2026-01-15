using System.Collections.Generic;

namespace DotNpm {

    public sealed class Sources {
        public IReadOnlySet<SourceFileBase> Files { get; internal set; }
    }
}
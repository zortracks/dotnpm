using System.Collections.Generic;

namespace DotNpm {

    public sealed class Sources {
        public IEnumerable<SourceFileBase> Files { get; internal set; }
    }
}
using System.Collections.Generic;

namespace DotNpm {

    public sealed class BuiltInCssSourceFile : CssSourceFileBase {
    }

    public abstract class CssSourceFileBase : SourceFileBase {
        public IEnumerable<string> Imports { get; internal set; }
    }

    public sealed class LocalCssSourceFile : CssSourceFileBase {
    }
}
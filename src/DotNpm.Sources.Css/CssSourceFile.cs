using System.Collections.Generic;

namespace DotNpm {

    public sealed class BuiltInCssSourceFile : CssSourceFileBase {
    }

    public abstract class CssSourceFileBase : SourceFileBase {
        public HashSet<string> Imports { get; } = new HashSet<string>();
    }

    public sealed class LocalCssSourceFile : CssSourceFileBase {
    }
}
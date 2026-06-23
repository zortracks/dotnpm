using System.Collections.Generic;
using System.IO;

namespace DotNpm.Contexts {

    internal class NodeEnvironmentContext {
        public string PackageName { get; internal set; }
        public DirectoryInfo WorkingDirectory { get; internal set; }
    }
}
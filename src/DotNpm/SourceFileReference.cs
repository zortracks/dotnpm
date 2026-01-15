using System.IO;

namespace DotNpm {

    public sealed class SourceFileReference<TSourceFile> : IScriptReference
        where TSourceFile : SourceFileBase {

        public SourceFileReference(FileInfo sourceFile) {
            SourceFile = sourceFile;
        }

        public FileInfo SourceFile { get; }

        public string GetScriptReference() {
            throw new System.NotImplementedException();
        }
    }
}
using System.IO;
using System.Threading.Tasks;

namespace DotNpm {

    public sealed class OutputFileReference<TOutputFile> : IScriptReference
        where TOutputFile : OutputFileBase {
        public OutputFileReference(FileInfo targetFile) {
            TargetFile = targetFile;
        }

        public FileInfo TargetFile { get; }
        public Task EnsureCreatedAsync(ScriptInvocationReference scriptInvocationReference) {
            if (!TargetFile.Exists || scriptInvocationReference.Status == TaskStatus.WaitingForActivation)
                return scriptInvocationReference.RunAsync();
            else
                return Task.CompletedTask;
        }

        public string GetScriptReference() {
            return string.Empty;
        }
    }
}
using System.Threading.Tasks;

namespace DotNpm {

    internal sealed class NodeAsset<TOutputFile> : INodeAsset
        where TOutputFile : OutputFileBase {
        private readonly OutputFileReference<TOutputFile> _outputReference;
        private readonly ScriptInvocationReference _scriptInvocationReference;

        public NodeAsset(OutputFileReference<TOutputFile> outputReference, ScriptInvocationReference scriptInvocationReference) {
            _outputReference = outputReference;
            _scriptInvocationReference = scriptInvocationReference;
        }

        public Task EnsureReadyAsync() => _outputReference.EnsureCreatedAsync(_scriptInvocationReference);
    }
}
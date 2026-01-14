namespace DotNpm {

    public interface ISourceFileReference<TSourceFile> : IScriptReference
        where TSourceFile : SourceFileBase {
    }
}
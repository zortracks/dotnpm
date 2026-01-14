namespace DotNpm {

    public interface IOutputFileReference<TOutputFile> : IScriptReference
        where TOutputFile : OutputFileBase {
    }
}
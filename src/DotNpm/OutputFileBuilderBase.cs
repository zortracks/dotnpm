using System;

namespace DotNpm {

    public abstract class OutputFileBuilderBase<T, TOutputFile>
        where T : OutputFileBuilderBase<T, TOutputFile>
        where TOutputFile : OutputFileBase {
        public bool Watch { get; private set; }

        public abstract OutputFileBase GetOutputFile(IServiceProvider serviceProvider);

        public abstract IOutputFileReference<TOutputFile> GetOutputFileReference();

        public T WithFileWatching(bool watch = true) {
            Watch = watch;

            return (T)this;
        }
    }
}
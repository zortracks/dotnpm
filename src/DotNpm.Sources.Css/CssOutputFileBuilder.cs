using System;
using System.IO;

namespace DotNpm {

    public sealed class CssOutputFileBuilder : OutputFileBuilderBase<CssOutputFileBuilder, CssOutputFile> {
        private readonly DirectoryInfo _baseDirectory;
        private readonly string _fileName;

        public CssOutputFileBuilder(string fileName, DirectoryInfo baseDirectory) {
            _fileName = fileName;
            _baseDirectory = baseDirectory;
        }

        public override OutputFileBase GetOutputFile(IServiceProvider serviceProvider) {
            throw new NotImplementedException();
        }

        public override OutputFileReference<CssOutputFile> GetOutputFileReference() => new OutputFileReference<CssOutputFile>(new FileInfo(Path.Combine(_baseDirectory.FullName, _fileName)));
    }
}
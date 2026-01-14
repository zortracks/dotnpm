using System;

namespace DotNpm {

    public sealed class CssOutputFileBuilder : OutputFileBuilderBase<CssOutputFileBuilder, CssOutputFile> {
        private string _fileName;

        public CssOutputFileBuilder(string fileName) {
            _fileName = fileName;
        }

        public override OutputFileBase GetOutputFile(IServiceProvider serviceProvider) {
            throw new NotImplementedException();
        }

        public override IOutputFileReference<CssOutputFile> GetOutputFileReference() {
            return null;
        }
    }
}
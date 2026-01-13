using System;
using System.IO;

namespace DotNpm {

    public sealed class CssSourceFileBuilder : SourceFileBuilder<BuiltInCssSourceFile> {

        public CssSourceFileBuilder(IServiceProvider serviceProvider, DirectoryInfo baseDirectory, string fileName) : base(serviceProvider, baseDirectory, fileName) {
        }

        public CssSourceFileBuilder WithImport(string import) {
            SourceFile.Imports.Add(import);

            return this;
        }
    }
}
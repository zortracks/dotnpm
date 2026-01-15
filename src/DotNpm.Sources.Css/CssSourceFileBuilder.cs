using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNpm {

    public sealed class CssSourceFileBuilder : SourceFileBuilderBase<BuiltInCssSourceFile> {

        public CssSourceFileBuilder(IServiceCollection services, DirectoryInfo baseDirectory, string fileName) : base(services, baseDirectory, fileName) {
        }

        public HashSet<Func<IServiceProvider, string>> Imports { get; } = new HashSet<Func<IServiceProvider, string>>();

        public override BuiltInCssSourceFile GetSourceFile(IServiceProvider serviceProvider) {
            var builtInCssSourceFile = ActivatorUtilities.CreateInstance<BuiltInCssSourceFile>(serviceProvider);

            builtInCssSourceFile.Imports = Imports.Select(import => import.Invoke(serviceProvider));

            return builtInCssSourceFile;
        }

        public override SourceFileReference<BuiltInCssSourceFile> GetSourceFileReference() {
            return new SourceFileReference<BuiltInCssSourceFile>(new FileInfo(Path.Combine(_baseDirectory.FullName, _fileName)));
        }

        public CssSourceFileBuilder WithImport(string import) {
            Imports.Add(_ => import);

            return this;
        }
    }
}
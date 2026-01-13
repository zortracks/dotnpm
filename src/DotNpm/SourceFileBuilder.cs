using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DotNpm {

    public abstract class SourceFileBuilder<TSourceFile> where TSourceFile : SourceFileBase {
        protected readonly DirectoryInfo _baseDirectory;
        protected readonly IServiceProvider _serviceProvider;

        protected SourceFileBuilder(IServiceProvider serviceProvider, DirectoryInfo baseDirectory, string fileName) {
            _serviceProvider = serviceProvider;
            _baseDirectory = baseDirectory;

            SourceFile = ActivatorUtilities.CreateInstance<TSourceFile>(_serviceProvider);
            SourceFile.FilePath = Path.Combine(_baseDirectory.FullName, fileName);
        }

        public TSourceFile SourceFile { get; }
    }
}
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DotNpm {

    public abstract class SourceFileBuilderBase<TSourceFile> where TSourceFile : SourceFileBase {
        protected readonly DirectoryInfo _baseDirectory;
        protected readonly string _fileName;
        protected readonly IServiceCollection _services;

        protected SourceFileBuilderBase(IServiceCollection services, DirectoryInfo baseDirectory, string fileName) {
            _services = services;
            _baseDirectory = baseDirectory;
            _fileName = fileName;
        }

        public abstract TSourceFile GetSourceFile(IServiceProvider serviceProvider);

        public abstract ISourceFileReference<TSourceFile> GetSourceFileReference();
    }
}
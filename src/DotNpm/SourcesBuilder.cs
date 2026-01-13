using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DotNpm {

    public sealed class SourcesBuilder {
        private readonly DirectoryInfo _baseDirectory;
        private IServiceProvider _serviceProvider;

        public SourcesBuilder(IServiceProvider serviceProvider, DirectoryInfo baseDirectory) {
            _serviceProvider = serviceProvider;
            _baseDirectory = baseDirectory;

            Sources = ActivatorUtilities.CreateInstance<Sources>(_serviceProvider);
        }

        public Sources Sources { get; }

        public SourcesBuilder AddFile<TSourceFileBuilder, TSourceFile>(string fileName, Action<TSourceFileBuilder> builder, out TSourceFile file)
            where TSourceFile : SourceFileBase
            where TSourceFileBuilder : SourceFileBuilder<TSourceFile> {
            var sourceFileBuilder = ActivatorUtilities.CreateInstance<TSourceFileBuilder>(_serviceProvider, _baseDirectory, fileName);

            builder.Invoke(sourceFileBuilder);
            Sources.Files.Add(file = sourceFileBuilder.SourceFile);

            return this;
        }
    }
}
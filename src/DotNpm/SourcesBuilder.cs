using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNpm {

    public sealed class SourcesBuilder {
        public readonly DirectoryInfo _baseDirectory;
        public readonly IServiceCollection _services;

        public SourcesBuilder(IServiceCollection services, DirectoryInfo baseDirectory) {
            _services = services;
            _baseDirectory = baseDirectory;
        }

        public HashSet<Func<IServiceProvider, SourceFileBase>> Files { get; } = new HashSet<Func<IServiceProvider, SourceFileBase>>();

        public Sources GetSources(IServiceProvider serviceProvider) {
            var sources = ActivatorUtilities.CreateInstance<Sources>(serviceProvider);

            sources.Files = Files.Select(file => file.Invoke(serviceProvider));

            return sources;
        }
    }
}
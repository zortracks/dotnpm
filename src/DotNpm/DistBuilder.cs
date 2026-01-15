using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNpm {

    public sealed class DistBuilder {
        public readonly DirectoryInfo _baseDirectory;
        public readonly IServiceCollection _services;

        public DistBuilder(IServiceCollection services, DirectoryInfo baseDirectory) {
            _services = services;
            _baseDirectory = new DirectoryInfo(Path.Combine(baseDirectory.FullName, "dist"));
        }

        public HashSet<Func<IServiceProvider, OutputFileBase>> FilesFactory { get; } = new HashSet<Func<IServiceProvider, OutputFileBase>>();

        public Dist GetDist(IServiceProvider serviceProvider) {
            var dist = ActivatorUtilities.CreateInstance<Dist>(serviceProvider);

            return dist;
        }
    }
}
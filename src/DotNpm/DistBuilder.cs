using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DotNpm {

    public sealed class DistBuilder {
        private IServiceCollection _services;

        public DistBuilder(IServiceCollection services) {
            _services = services;
        }

        public HashSet<Func<IServiceProvider, OutputFileBase>> FilesFactory { get; } = new HashSet<Func<IServiceProvider, OutputFileBase>>();

        public Dist GetDist(IServiceProvider serviceProvider) {
            var dist = ActivatorUtilities.CreateInstance<Dist>(serviceProvider);

            return dist;
        }
    }
}
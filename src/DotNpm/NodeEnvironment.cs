using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DotNpm {

    public sealed class NodeEnvironment {
        private readonly ILogger<NodeEnvironment> _logger;

        public NodeEnvironment(ILogger<NodeEnvironment> logger) {
            _logger = logger;
        }

        public DirectoryInfo Directory { get; internal set; }
        public string Name { get; internal set; }
        public IPackage Package { get; internal set; }

        internal IServiceProvider ServiceProvider { get; set; }

        public async Task RunAsync(CancellationToken cancellationToken = default) {
            await Task.WhenAll(Package.PrepareAsync(Directory, cancellationToken));
        }
    }
}
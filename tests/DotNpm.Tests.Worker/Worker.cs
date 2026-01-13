using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DotNpm.Tests.Worker {

    public sealed class Worker : BackgroundService {
        private readonly NodeEnvironment _builtIn;

        public Worker([FromKeyedServices("built-in")] NodeEnvironment builtIn) {
            _builtIn = builtIn;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => _builtIn.RunAsync(stoppingToken);
    }
}
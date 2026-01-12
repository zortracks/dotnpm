using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DotNpm.Tests.Worker {

    public sealed class Worker : BackgroundService {
        private readonly NodeEnvironment _builtIn;
        private readonly NodeEnvironment _local;

        public Worker([FromKeyedServices("local")] NodeEnvironment local, [FromKeyedServices("built-in")] NodeEnvironment builtIn) {
            _local = local;
            _builtIn = builtIn;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) {
            Task.WaitAll(_local.RunAsync(stoppingToken), _builtIn.RunAsync(stoppingToken));

            return Task.CompletedTask;
        }
    }
}
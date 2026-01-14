using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNpm {

    public sealed class NodeAssetsService {
        private readonly NodeEnvironment _environment;

        public NodeAssetsService(IServiceProvider serviceProvider, string environmentName) {
            _environment = serviceProvider.GetKeyedService<NodeEnvironment>(environmentName);
        }

        public string GetAssetReference(string assetName) {
            throw new NotImplementedException();
        }
    }
}
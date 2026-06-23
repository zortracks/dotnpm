using DotNpm.Elements;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DotNpm {

    public sealed class NodeAssetsService {
        private readonly NodeEnvironment _environment;

        public NodeAssetsService(IServiceProvider serviceProvider, string environmentName) {
            _environment = serviceProvider.GetKeyedService<NodeEnvironment>(environmentName);
        }

        public async Task<string> GetAssetReferenceAsync(string assetName) {
            var asset = GetAsset(assetName);

            await asset.EnsureReadyAsync();

            return string.Empty;
        }

        private INodeAsset GetAsset(string assetName) => _environment.Assets[Path.GetRelativePath("./", assetName)];
    }
}
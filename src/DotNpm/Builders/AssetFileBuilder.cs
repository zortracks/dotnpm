using Microsoft.Extensions.DependencyInjection;

namespace DotNpm.Builders {

    public sealed class AssetFileBuilder : FileBuilder {

        public AssetFileBuilder(IServiceCollection services, string packageName, string fileName) : base(services, fileName) {
        }
    }
}
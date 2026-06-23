using Microsoft.Extensions.DependencyInjection;

namespace DotNpm.Builders {

    public abstract class FileBuilder {
        protected readonly string _fileName;
        protected readonly IServiceCollection _services;

        protected FileBuilder(IServiceCollection services, string fileName) {
            _services = services;
            _fileName = fileName;
        }
    }
}
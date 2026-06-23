using DotNpm.Elements;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace DotNpm.Builders {

    public sealed class ScriptBuilder {
        private readonly string _scriptName;
        private readonly IServiceCollection _services;

        public ScriptBuilder(IServiceCollection services, string scriptName) {
            _services = services;
            _scriptName = scriptName;

            _services.Configure<Script>(_scriptName, script => script.Name =new FileInfo( _scriptName).Name);
        }
    }
}
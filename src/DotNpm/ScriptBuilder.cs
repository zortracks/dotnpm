using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNpm {

    public sealed class ScriptBuilder {
        private readonly string _scriptName;
        private readonly IServiceProvider _serviceProvider;

        public ScriptBuilder(IServiceProvider serviceProvider, string scriptName) {
            _serviceProvider = serviceProvider;
            _scriptName = scriptName;
        }

        public HashSet<IScriptReference> References { get; } = new HashSet<IScriptReference>();

        public BuiltInScript Script {
            get => new BuiltInScript(_serviceProvider, _scriptName) {
                Command = string.Join(' ', References.Select(reference => reference.GetScriptReference()))
            };
        }

        public ScriptBuilder Inline(string inline) {
            References.Add(new InlineScriptReference(inline));

            return this;
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNpm {

    public sealed class ScriptBuilder {
        private readonly string _scriptName;
        private readonly IServiceCollection _services;

        public ScriptBuilder(IServiceCollection services, string scriptName) {
            _services = services;
            _scriptName = scriptName;
        }

        public HashSet<Func<IServiceProvider, IScriptReference>> ReferencesFactory { get; } = new HashSet<Func<IServiceProvider, IScriptReference>>();

        public BuiltInScript GetScript(IServiceProvider serviceProvider) {
            var builtInScript = ActivatorUtilities.CreateInstance<BuiltInScript>(serviceProvider, _scriptName);

            builtInScript.Command = string.Join(' ', ReferencesFactory.Select(reference => reference.Invoke(serviceProvider).GetScriptReference()).Where(reference => !string.IsNullOrEmpty(reference)));

            return builtInScript;
        }

        public IScriptInvocationReference GetScriptInvocationReference() {
            return null;
        }

        public ScriptBuilder Inline(string inline) {
            ReferencesFactory.Add(_ => new InlineScriptReference(inline));

            return this;
        }

        public ScriptBuilder WithOutputFileReference<TOutputFile>(IOutputFileReference<TOutputFile> reference) where TOutputFile : OutputFileBase {
            ReferencesFactory.Add(_ => reference);

            return this;
        }

        public ScriptBuilder WithSourceFileReference<TSourceFile>(ISourceFileReference<TSourceFile> reference) where TSourceFile : SourceFileBase {
            ReferencesFactory.Add(_ => reference);

            return this;
        }

        public ScriptBuilder WithWatching(string inline, bool watch = true) {
            if (watch)
                ReferencesFactory.Add(_ => new InlineScriptReference(inline));

            return this;
        }
    }
}
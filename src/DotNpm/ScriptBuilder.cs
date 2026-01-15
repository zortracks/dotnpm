using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNpm {

    public sealed class ScriptBuilder {
        private readonly DirectoryInfo _baseDirectory;
        private readonly string _scriptName;
        private readonly IServiceCollection _services;
        private BuiltInScript _builtInScript;

        public ScriptBuilder(IServiceCollection services, DirectoryInfo baseDirectory, string scriptName) {
            _services = services;
            _scriptName = scriptName;
            _baseDirectory = baseDirectory;
        }

        public HashSet<Func<IServiceProvider, IScriptReference>> ReferencesFactory { get; } = new HashSet<Func<IServiceProvider, IScriptReference>>();
        public bool Watch { get; private set; }

        public BuiltInScript GetScript(IServiceProvider serviceProvider) {
            _builtInScript = ActivatorUtilities.CreateInstance<BuiltInScript>(serviceProvider, _scriptName);

            _builtInScript.Command = string.Join(' ', ReferencesFactory.Select(reference => reference.Invoke(serviceProvider).GetScriptReference()).Where(reference => !string.IsNullOrEmpty(reference)));
            _builtInScript.Watch = Watch;
            _builtInScript.Directory = _baseDirectory;

            return _builtInScript;
        }

        public ScriptInvocationReference GetScriptInvocationReference() => new ScriptInvocationReference(() => _builtInScript.InvokeAsync());

        public ScriptBuilder Inline(string inline) {
            ReferencesFactory.Add(_ => new InlineScriptReference(inline));

            return this;
        }

        public ScriptBuilder WithOutputFileReference<TOutputFile>(OutputFileReference<TOutputFile> reference) where TOutputFile : OutputFileBase {
            ArgumentNullException.ThrowIfNull(reference, nameof(reference));
            ReferencesFactory.Add(_ => reference);

            return this;
        }

        public ScriptBuilder WithSourceFileReference<TSourceFile>(SourceFileReference<TSourceFile> reference) where TSourceFile : SourceFileBase {
            ArgumentNullException.ThrowIfNull(reference, nameof(reference));
            ReferencesFactory.Add(_ => reference);

            return this;
        }

        public ScriptBuilder WithWatching(string inline, bool watch = true) {
            if (Watch = watch) {
                ReferencesFactory.Add(_ => new InlineScriptReference(inline));
            }

            return this;
        }
    }
}
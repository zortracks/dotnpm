using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DotNpm {

    public sealed class BuiltInScript : ScriptBase {

        public BuiltInScript(IServiceProvider serviceProvider, string name) : base(serviceProvider, name) {
        }

        internal sealed class BuiltInScriptConverter : JsonConverter<IEnumerable<ScriptBase>> {

            public override IEnumerable<ScriptBase> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, IEnumerable<ScriptBase> value, JsonSerializerOptions options) {
                writer.WriteStartObject();

                foreach (var dependency in value) {
                    writer.WritePropertyName(dependency.Name);
                    writer.WriteStringValue(dependency.Command);
                }

                writer.WriteEndObject();
            }
        }
    }

    public sealed class LocalScript : ScriptBase {

        public LocalScript(IServiceProvider serviceProvider, string name) : base(serviceProvider, name) {
        }

        internal sealed class LocalScriptConverter : JsonConverter<IEnumerable<ScriptBase>> {
            private readonly DirectoryInfo _directory;
            private readonly IServiceProvider _serviceProvider;
            private readonly IEnumerable<string> _watchParameters;

            public LocalScriptConverter(IServiceProvider serviceProvider, DirectoryInfo directory, IEnumerable<string> watchParameters) {
                _serviceProvider = serviceProvider;
                _directory = directory;
                _watchParameters = watchParameters;
            }

            public override IEnumerable<ScriptBase> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                var document = JsonDocument.ParseValue(ref reader).RootElement;
                var result = new HashSet<ScriptBase>();

                foreach (var field in document.EnumerateObject()) {
                    var localScript = ActivatorUtilities.CreateInstance<LocalScript>(_serviceProvider, field.Name);

                    localScript.Command = field.Value.GetString();
                    localScript.Directory = _directory;
                    localScript.Watch = _watchParameters.Any(localScript.Command.Contains);

                    result.Add(localScript);
                }
                return result;
            }

            public override void Write(Utf8JsonWriter writer, IEnumerable<ScriptBase> value, JsonSerializerOptions options) {
                writer.WriteStartObject();

                foreach (var dependency in value) {
                    writer.WritePropertyName(dependency.Name);
                    writer.WriteStringValue(dependency.Command);
                }

                writer.WriteEndObject();
            }
        }
    }

    public abstract class ScriptBase {
        private readonly IServiceProvider _serviceProvider;

        protected ScriptBase(IServiceProvider serviceProvider, string name) {
            _serviceProvider = serviceProvider;
            Name = name;
        }

        public string Command { get; internal set; }
        public DirectoryInfo Directory { get; internal set; }
        public string Name { get; }
        public bool Watch { get; internal set; }

        public async Task InvokeAsync() {
            _serviceProvider.GetRequiredService<NodeInvocationService>().CreateContext(Directory).RunAsync(Name);
        }
    }
}
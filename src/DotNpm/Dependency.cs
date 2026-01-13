using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotNpm {

    public sealed class Dependency {

        public Dependency(string name) {
            Name = name;
        }

        public string Name { get; }
        public string Version { get; internal set; }

        internal sealed class DependencyConverter : JsonConverter<IEnumerable<Dependency>> {

            public override IEnumerable<Dependency> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                var document = JsonDocument.ParseValue(ref reader).RootElement;
                var result = new HashSet<Dependency>();

                foreach (var field in document.EnumerateObject())
                    result.Add(new Dependency(field.Name) {
                        Version = field.Value.GetString()
                    });

                return result;
            }

            public override void Write(Utf8JsonWriter writer, IEnumerable<Dependency> value, JsonSerializerOptions options) {
                writer.WriteStartObject();

                foreach (var dependency in value) {
                    writer.WritePropertyName(dependency.Name);
                    writer.WriteStringValue(dependency.Version);
                }

                writer.WriteEndObject();
            }
        }
    }
}
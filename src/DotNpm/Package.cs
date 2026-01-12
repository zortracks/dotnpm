using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace DotNpm {

    public interface IPackage {
        IDictionary<string, string> DevDependencies { get; }
        string Name { get; }
        string Version { get; }

        Task PrepareAsync(DirectoryInfo baseDirectory, CancellationToken cancellationToken);
    }

    public sealed class BuiltInPackage : PackageBase<BuiltInPackage>, IPackage {

        public BuiltInPackage(ILogger<BuiltInPackage> logger, NodeInvocationService nodeInvocationService) : base(logger, nodeInvocationService) {
        }

        public IDictionary<string, string> DevDependencies { get; } = new Dictionary<string, string>();
        public string Name { get; internal set; }
        public string Version { get; internal set; }

        public Task PrepareAsync(DirectoryInfo baseDirectory, CancellationToken cancellationToken) {
            _logger.LogDebug("Preparing package '{0}'", Name);

            return Task.Run(async () => {
                await WritePackage(baseDirectory);
                await InstallAsync(baseDirectory, Name, cancellationToken);
            }).ContinueWith(task => {
                if (task.Status == TaskStatus.RanToCompletion)
                    _logger.LogDebug("Package '{0}' prepared successfully.", Name);
                else
                    _logger.LogError(task.Exception, "Package '{0}' preparation failed.", Name);
            });
        }

        private Task WritePackage(DirectoryInfo baseDirectory) {
            _logger.LogDebug("Writing package '{0}'", Name);

            if (!baseDirectory.Exists)
                baseDirectory.Create();

            return File.WriteAllTextAsync(Path.Combine(baseDirectory.FullName, "package.json"), JsonSerializer.Serialize(this, new JsonSerializerOptions() {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }

    public abstract class PackageBase<T> where T : PackageBase<T> {
        protected readonly ILogger<T> _logger;
        private readonly NodeInvocationService _nodeInvocationService;

        protected PackageBase(ILogger<T> logger, NodeInvocationService nodeInvocationService) {
            _logger = logger;
            _nodeInvocationService = nodeInvocationService;
        }

        protected Task InstallAsync(DirectoryInfo baseDirectory, string packageName, CancellationToken cancellationToken) {
            _logger.LogDebug("Installing dependencies for package '{0}'", packageName);

            return _nodeInvocationService.CreateContext(baseDirectory).InstallAsync().ContinueWith(task => {
                if (task.Status == TaskStatus.RanToCompletion)
                    _logger.LogDebug("Dependencies installed successfully for package '{0}'.", packageName);
                else
                    _logger.LogError(task.Exception, "Dependencies installation failed for package '{0}'", packageName);
            });
        }
    }

    internal sealed class LocalPackage : PackageBase<LocalPackage>, IPackage {

        public LocalPackage(ILogger<LocalPackage> logger, NodeInvocationService nodeInvocationService) : base(logger, nodeInvocationService) {
        }

        public IDictionary<string, string> DevDependencies { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public Task PrepareAsync(DirectoryInfo baseDirectory, CancellationToken cancellationToken) {
            _logger.LogDebug("Preparing package '{0}'", Name);

            return InstallAsync(baseDirectory, Name, cancellationToken).ContinueWith(task => {
                if (task.Status == TaskStatus.RanToCompletion)
                    _logger.LogDebug("Package '{0}' prepared successfully.", Name);
                else
                    _logger.LogError(task.Exception, "Package '{0}' preparation failed.", Name);
            });
        }

        internal sealed class LocalPackageConverter : JsonConverter<LocalPackage> {
            private readonly IServiceProvider _serviceProvider;

            public LocalPackageConverter(IServiceProvider serviceProvider) {
                _serviceProvider = serviceProvider;
            }

            public override LocalPackage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                var document = JsonDocument.ParseValue(ref reader).RootElement;
                var localPackage = ActivatorUtilities.CreateInstance<LocalPackage>(_serviceProvider);

                foreach (var property in typeof(LocalPackage).GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    if (document.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(property.Name), out var value))
                        property.SetValue(localPackage, value.Deserialize(property.PropertyType, options));

                return localPackage;
            }

            public override void Write(Utf8JsonWriter writer, LocalPackage value, JsonSerializerOptions options) {
                options.Converters.Remove(this);
                JsonSerializer.Serialize(writer, value, options);
            }
        }
    }
}
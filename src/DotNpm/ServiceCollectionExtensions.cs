using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace DotNpm {

    public static class ServiceCollectionExtensions {

        public static IServiceCollection AddDotNpm(this IServiceCollection services, string name, Action<NodeEnvironmentBuilder> builder) {
            services.AddDotNpmCore();
            services.TryAddKeyedSingleton(name, (serviceProvider, _) => {
                var nodeEnvironmentBuilder = new NodeEnvironmentBuilder(serviceProvider, name);

                builder.Invoke(nodeEnvironmentBuilder);

                return nodeEnvironmentBuilder.Environment;
            });

            return services;
        }

        public static IServiceCollection AddDotNpmCore(this IServiceCollection services) {
            services.AddSingleton<NodeInvocationService>();

            return services;
        }
    }
}
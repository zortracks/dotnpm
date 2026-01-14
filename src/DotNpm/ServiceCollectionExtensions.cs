using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace DotNpm {

    public static class ServiceCollectionExtensions {

        public static IServiceCollection AddDotNpm(this IServiceCollection services, string environmentName, Action<NodeEnvironmentBuilder> builder) {
            var nodeEnvironmentBuilder = new NodeEnvironmentBuilder(services, environmentName);

            builder.Invoke(nodeEnvironmentBuilder);

            services.AddDotNpmCore();
            services.TryAddKeyedSingleton(environmentName, (serviceProvider, _) => nodeEnvironmentBuilder.GetEnvironment(serviceProvider));
            services.TryAddKeyedSingleton(environmentName, (serviceProvider, _) => ActivatorUtilities.CreateInstance<NodeAssetsService>(serviceProvider, environmentName));

            return services;
        }

        public static IServiceCollection AddDotNpmCore(this IServiceCollection services) {
            services.AddSingleton<NodeInvocationService>();

            return services;
        }
    }
}
using DotNpm.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNpm {

    public static class ServiceCollectionExtensions {

        public static IServiceCollection AddDotNpm(this IServiceCollection services, string environmentName, Action<NodeEnvironmentBuilder> builder) {
            builder.Invoke(new NodeEnvironmentBuilder(services, environmentName));

            services.AddDotNpmCore();

            return services;
        }

        public static IServiceCollection AddDotNpmCore(this IServiceCollection services) {
            services.AddSingleton<NodeInvocationService>();

            return services;
        }
    }
}
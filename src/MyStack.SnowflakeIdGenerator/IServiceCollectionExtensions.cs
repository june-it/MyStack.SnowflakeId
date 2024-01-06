using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyStack.SnowflakeIdGenerator
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSnowflakeId(this IServiceCollection services, Action<SnawflakeIdOptions> configure )
        {
            var options = new SnawflakeIdOptions();
            configure?.Invoke(options);
            services.Configure(configure);
            services.AddSingleton<ISnowflakeId, SnowflakeId>();
            return services;
        }
        public static IServiceCollection AddSnowflakeId(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection("SnowflakeIdGenerator");
            var options = new SnawflakeIdOptions();
            configurationSection.Bind(options);
            services.Configure<SnawflakeIdOptions>(configurationSection); 
            services.AddSingleton<ISnowflakeId, SnowflakeId>();
            return services;
        }
    }
}

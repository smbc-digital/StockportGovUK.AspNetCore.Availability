using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StockportGovUK.AspNetCore.Availability.Managers;
using StockportGovUK.AspNetCore.Availability.Builders;

namespace StockportGovUK.AspNetCore.Availability
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAvailability(this IServiceCollection services)
        {
            services.AddSingleton<IAvailabilityManager, AvailabilityManager>();
        }

        public static IAvailabilityBuilder AddAvailabilityClient(this IServiceCollection services)
        {
            services.Configure<AvailabilityOptions>(options => options.CacheProviders = new List<string>());
            services.AddSingleton<IAvailabilityManager>(provider =>
            {
                var clientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var options = provider.GetRequiredService<IOptions<AvailabilityOptions>>();
                return new AvailabilityManager(clientFactory, configuration, options.Value);
            });

            return new AvailabilityBuilder(services);
        }

        public static IAvailabilityBuilder WithRedisCache(this IAvailabilityBuilder builder)
        {
            if (builder == null) throw new ArgumentException(nameof(builder));

            builder.Services.Configure<AvailabilityOptions>(options => options.CacheProviders.Add("Redis"));
            return builder;
        }

        public static IAvailabilityBuilder WithSessionCache(this IAvailabilityBuilder builder)
        {
            if (builder == null) throw new ArgumentException(nameof(builder));

            builder.Services.Configure<AvailabilityOptions>(options => options.CacheProviders.Add("Session"));
            return builder;
        }
    }
}
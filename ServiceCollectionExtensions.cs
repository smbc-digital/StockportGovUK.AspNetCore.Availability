using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockportGovUK.AspNetCore.Availability.Managers;

namespace StockportGovUK.AspNetCore.Availability
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAvailability(this IServiceCollection services)
        {
            services.AddSingleton<IAvailabilityManager, AvailabilityManager>();
        }
    }
}
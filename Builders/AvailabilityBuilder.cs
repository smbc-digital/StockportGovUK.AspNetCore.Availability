using Microsoft.Extensions.DependencyInjection;

namespace StockportGovUK.AspNetCore.Availability.Builders
{
    public class AvailabilityBuilder : IAvailabilityBuilder
    {
        public AvailabilityBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
        
        public IServiceCollection Services { get; }
    }    
}

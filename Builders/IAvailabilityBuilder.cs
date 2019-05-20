using Microsoft.Extensions.DependencyInjection;

namespace StockportGovUK.AspNetCore.Availability.Builders
{
    public interface IAvailabilityBuilder
    {
        IServiceCollection Services { get; }
    } 
}
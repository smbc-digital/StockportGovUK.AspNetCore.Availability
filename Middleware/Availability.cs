using System.Threading.Tasks;
using StockportGovUK.AspNetCore.Availability.Managers;
using Microsoft.AspNetCore.Http;

namespace StockportGovUK.AspNetCore.Availability.Middleware
{
    public class Availability
    {
        private readonly RequestDelegate _next;
        private readonly IAvailabilityManager _availabilityManager;

        public Availability(RequestDelegate next, IAvailabilityManager availabilityManager)
        {
            _next = next;
            _availabilityManager = availabilityManager;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request != null)
            {
                var availabilityConfiguration = _availabilityManager.AvailabilityConfiguration;

                if (!availabilityConfiguration.Enabled)
                {
                    await _next.Invoke(context);
                }

                var appEnabled = await _availabilityManager.IsApplicationEnabled();

                if (!appEnabled && !availabilityConfiguration.IsAccessibleAddress(context.Request))
                {
                    context.Response.Redirect(availabilityConfiguration.GetErrorUrl(context));
                }
                else if (_next != null)
                {
                    await _next.Invoke(context);
                }
            }
        }
    }
}
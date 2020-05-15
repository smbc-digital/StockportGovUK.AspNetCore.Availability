using System;
using System.Net;
using System.Threading.Tasks;
using StockportGovUK.AspNetCore.Availability.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
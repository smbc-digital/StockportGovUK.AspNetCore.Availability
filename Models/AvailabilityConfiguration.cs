using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace StockportGovUK.AspNetCore.Availability.Models
{
    public class AvailabilityConfiguration
    {
        public string BaseUrl{ get; set; }
        public string ErrorRoute { get; set; }
        public List<string> WhitelistedRoutes { get; set; }
        public bool AllowSwagger { get; set; }
        public string Environment { get; set; }
        public string Key { get; set; }
        public bool Enabled { get; set; } = true;

        public string GetErrorUrl(HttpContext httpContext)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{ErrorRoute}";
        }

        public bool IsAccessibleAddress(HttpRequest request)
        {
            return (AllowSwagger && request.IsSwaggerRequest()) || WhitelistedRoutes.Contains(request.Path);
        }
    }
}

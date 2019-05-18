using System;
using Microsoft.AspNetCore.Http;

namespace StockportGovUK.AspNetCore.Availability
{
    public static class HttpRequestExtensions
    {
        public static bool IsSwaggerRequest(this HttpRequest httpRequest)
        {
            return httpRequest.Path.Value.StartsWith("/swagger", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
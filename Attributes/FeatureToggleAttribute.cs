using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using StockportGovUK.AspNetCore.Availability.Managers;

namespace StockportGovUK.AspNetCore.Availability.Attributes
{
    public class FeatureToggleAttribute : ActionFilterAttribute
    {
        private string ToggleName { get; set; }

        public FeatureToggleAttribute(string toggleName)
        {
            ToggleName = toggleName;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var availabilityManager = actionContext.HttpContext.RequestServices.GetService<IAvailabilityManager>(); 
            if (availabilityManager == null)
            {
                throw new Exception("Could not load Availability manager, ensure yoiu have registersted this with the DI container");
            }

            availabilityManager.RegisterFeature(ToggleName);
            var isFeatureEnabled = availabilityManager.IsFeatureEnabled(ToggleName).Result;

            if(!isFeatureEnabled)
            {
                actionContext.Result = new NotFoundObjectResult("Not Found");
            }
        }
    }
}

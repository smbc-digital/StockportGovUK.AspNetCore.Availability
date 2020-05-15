using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StockportGovUK.AspNetCore.Availability.Models;
using Microsoft.Extensions.Configuration;

namespace StockportGovUK.AspNetCore.Availability.Managers
{
    public interface IAvailabilityManager
    {
        Task<bool> IsApplicationEnabled();
        Task RegisterFeature(string featureName);
        Task RegisterOperation(string operationName);
        Task<bool> IsFeatureEnabled(string featureName);
        Task<bool> IsOperationEnabled(string operationName);
        AvailabilityConfiguration AvailabilityConfiguration { get; }
    }

    public class AvailabilityManager : IAvailabilityManager
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly string _appName;
        private readonly Guid _registrationId;
        public AvailabilityConfiguration AvailabilityConfiguration { get; private set; }
        public AvailabilityOptions AvailabilityOptions { get; set; }

        public AvailabilityManager(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _appName = Assembly.GetEntryAssembly().GetName().Name;
            AvailabilityConfiguration = GetConfiguration(configuration);
            _registrationId = RegisterApplication().Result;
        }

        public AvailabilityManager(IHttpClientFactory httpClientFactory, IConfiguration configuration, AvailabilityOptions availabilityOptions)
            : this(httpClientFactory, configuration)
        {
            AvailabilityOptions = availabilityOptions;
        }

        public async Task<bool> IsApplicationEnabled()
        {
            var result = await GetAsync($"{AvailabilityConfiguration.BaseUrl}/apps/{_registrationId}");
            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> IsFeatureEnabled(string featureName)
        {
            var result = await GetAsync($"{AvailabilityConfiguration.BaseUrl}/apps/{_registrationId}/features/name/{featureName}");
            return result.StatusCode == HttpStatusCode.OK;
        }
        
        public async Task<bool> IsOperationEnabled(string operationName)
        {
            var result = await GetAsync($"{AvailabilityConfiguration.BaseUrl}/apps/{_registrationId}/operations/name/{operationName}");
            return result.StatusCode == HttpStatusCode.OK;
        }
        
        public async Task RegisterFeature(string featureName)

        {
            var payload = JsonSerializer.Serialize(new {name = featureName, enabled = true });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            await PostAsync($"{AvailabilityConfiguration.BaseUrl}/apps/{_registrationId}/features", content);
        }

        public async Task RegisterOperation(string operationName)
        {
            var payload = JsonSerializer.Serialize(new {name = operationName, enabled = true });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            await PostAsync($"{AvailabilityConfiguration.BaseUrl}/apps/{_registrationId}/operations", content);
        }

        private async Task<Guid> RegisterApplication()
        {           
            var payload = JsonSerializer.Serialize(new {name = _appName, environment = AvailabilityConfiguration.Environment, enabled = true });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await PostAsync($"{AvailabilityConfiguration.BaseUrl}/apps", content);
            var appGuid = response.Headers.Location.Segments.Last();

            return new Guid(appGuid);
        }

        private async Task<HttpResponseMessage> PostAsync(string url, StringContent content)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(AvailabilityConfiguration.Key);
            return await client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(AvailabilityConfiguration.Key);
            return await client.GetAsync(url);
        }
        
        private AvailabilityConfiguration GetConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new Exception("Could not load configuration");
            }

            var availabilityConfiguration = new AvailabilityConfiguration();
            var availabilityConfigurationSection = configuration.GetSection("Availability");

            availabilityConfigurationSection.Bind(availabilityConfiguration);
            availabilityConfiguration.WhitelistedRoutes.Add(availabilityConfiguration.ErrorRoute);

            return availabilityConfiguration;
        }
    }

    public class AvailabilityOptions
    {
        public List<string> CacheProviders { get; set; }

        public AvailabilityOptions (){
            CacheProviders = new List<string>();
        }
    }
}
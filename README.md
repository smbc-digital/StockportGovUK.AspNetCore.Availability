<div align="center">
<h1>Stockport Availability Package</h1>
</div>

<div align="center">
  <strong>Enable access to Stockports availabiity and feature toggling services.</strong>
</div>

<br />

<div align="center">
  <sub>Built with :heart: by
  <a href="https://www.stockport.gov.uk">Stockport Council</a>
</div>

## Table of Contents
- [Purpose](#purpose)
- [Initilaization](#initilaization)
- [License](#license)

## Purpose
To allow easy access to and use of Stockport's availability & feature toggling service.

## Initilaization

Availability cofiguration should be added to appSettings as below. Note that Key should be kept secret.

```json
  "Availability": {
    "BaseUrl": "http://scnavaildev.stockport.gov.uk/api/v1",
    "Key": "<AccessKeyHere>",
    "ErrorRoute": "/error/500",
    "WhitelistedRoutes": [
      "/swagger/index.html"
    ],
    "Environment": "int",
    "AllowSwagger": true
  }
```

Initialise and register for use by the DI container in ConfigureServices in Startup.cs

```c#
        public void ConfigureServices(IServiceCollection services)
        {
            ...
            services.AddAvailability();
        }
```

## Usage

### Middleware
Availability can check whether your entire app is toggled on/off in middleware during the prcessing of every request. Register the availability middleware in Configure in Startup.cs

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
  ...
  app.UseMiddleware<Availability>();
  ...
}

```
### Attributes
Attributes can be used at controller and action level using 


```c#
[FeatureToggle("ToggleNameHere")]
or
[OperationalToggle("ToggleNameHere")]
public IActionResult MyAction()
...
```

### Best Practices
If feature toggles are going to be used in multiple places it is advised they are made available as constants.

```c#
namespace yournamespace
{
    public static class FeatureToggles
    {
        public const string MyToggle = "MyToggleName";
    }
}
```

Then referenced as below

```c#
[FeatureToggle(FeatureToggles.MyToggle)]
public IActionResult MyAction()
...
```

## License
[MIT](https://opensource.org/licenses/MIT)
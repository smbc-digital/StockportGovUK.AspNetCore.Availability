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

Add configuration

```json
  "Availability": {
    "BaseUrl": "http://scnavaildev.stockport.gov.uk/api/v1",
    "ErrorRoute": "/error/500",
    "WhitelistedRoutes": [
      "/swagger/index.html"
    ],
    "Environment": "int",
    "AllowSwagger": true
  }
```
Access Key stored as a Secret
```json
  "Availability": {
    "Key": "<AccessKeyHere>"
  }
```
Initialise in Startup 

```c#
services.AddAvailability();
```

## License
[MIT](https://opensource.org/licenses/MIT)
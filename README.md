# SiteDomainMapper from Config for Umbraco v10 #

This package makes it possible to configure the Umbraco SiteDomainMapper from appsettings.json instead of in code.

See [here](https://docs.umbraco.com/umbraco-cms/reference/routing/request-pipeline/outbound-pipeline#site-domain-mapper) for info on what the SiteDomainMapper is used for.

## Configuration ##

Edit `appsettings.json` to add a "DomainMapper" section. Your settings file should look like the below:

```json
  "DomainMapper": {
    "Sites": [
      {
        "SiteName": "production",
        "Domains": [ "cms.mysite.com", "www.mysite.com", "pre-prod.mysite.com" ]
      },
      {
        "SiteName": "preproduction",
        "Domains": [ "mysite-umbraco-alternative.azurewebsites.net", "mysite-web-alternative.azurewebsites.net" ]
      },
      {
        "SiteName": "staging",
        "Domains": [ "mysite-umbraco.azurewebsites.net", "mysite-web.azurewebsites.net" ]
      },
      {
        "SiteName": "development",
        "Domains": [ "mysite.dev" ]
      },
      {
        "SiteName": "local",
        "Domains": [ "localhost:44375" ]
      }
    ],
    "Bindings": [
      [ "production", "preproduction" ],
      [ "staging", "development" ]
    ]
  }
```

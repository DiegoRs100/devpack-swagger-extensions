# What does it do?

This library is a facilitator for configuring the swagger in APIS.

# How to use it?

To use the library, just perform the following configuration at application startup:

```csharp
    public void ConfigureServices(IServiceCollection services, IHostEnvironment env)
    {
        ..
        services.AddSwaggerConfig(builder.Configuration, env);
    }

    public void Configure(IApplicationBuilder app)
    {
        ..
        app.UseSwaggerDefaultConfig();
    }
```

In addition, it is necessary to add the following structure in the **appsettings.json** of the application:

```json
    "Swagger": {
        "Title": "API title",
        "Description": "What does it do",
        "ReadmeUrl": ": "Project readme url",
        "ContactName": "Name of the person responsible for the API",
        "ContactEmail": "E-Mail of the person responsible for the API"
    }
```

# Features

#### Response Types

By default, the library already identifies the 400, 401 and 500 returns automatically, not being necessary to inform them.

#### Enum Descriptions

By default the library intercepts the parameters needed for each endpoint and translates the enum values ​​so that they can be displayed in the swagger interface.
To customize the description of the enums, just decorate each value with the **Description** decorator:

```csharp
    public enum Gender
    {
        None = 0

        [Description("Man")]
        Male = 2,

        [Description("Woman")]
        Female = 2
    }
```

In the interface, the following text will be generated over the gender parameter:

0 - None | 1 - Man | 2 - Woman

#### Readonly Properties

By default, the library maps all readonly parameters to endpoints and removes them from the swagger contract, thus improving its readability.

#### Production protection

By default, the application will remove all contract information when the application is published in production.
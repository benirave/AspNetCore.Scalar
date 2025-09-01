[![Nuget](https://img.shields.io/nuget/v/aspnetcore.scalar)](https://www.nuget.org/packages/AspNetCore.Scalar)

# .NET Scalar API Reference

Add the [Scalar API Reference](https://github.com/scalar/scalar?tab%253Dreadme-ov-file#with-nextjs) to any of your .NET applications.

> dotnet add package AspNetCore.Scalar --version 1.1.8

## Usage

To incorporate the Scalar UI, simply expose an OpenAPI schema by using [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.WebApi) or [NSwag](https://github.com/RicoSuter/NSwag), and then invoke the **app.UseScalar()**.

Code example.:

```csharp
using AspNetCore.Scalar;

var builder = WebApplication.CreateBuilder(args);

// Add swagger schema generator
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add swagger middleware
app.UseSwagger();
// Add Scalar UI
app.UseScalar(options =>
{
    options.UseTheme(Theme.Solarized);
});

```

Explore a working example [here](./example/).

### Customization Options

`InjectStylesheet`

- **Description**: Injects a stylesheet link into the head content of the ScalarOptions.
- **Parameters**:
  - `path` (string): The path to the stylesheet file.
  - `media` (string, optional): The media type for which the stylesheet is intended. Default value is "screen".

`UseSpecUrl`

- **Description**: Sets the URL for the specification page.
- **Parameters**:
  - `url` (string): The URL of the specification page.

`UseLayout`

- **Description**: Sets the layout for the ScalarOptions. You can use 'Modern' and Classic.
- **Default**: Modern
- **Parameters**:
  - `layout` (Theme): An enum representing the layout to use.

`UseTheme`

- **Description**: Sets the theme for the ScalarOptions.
- **Parameters**:
  - `theme` (Theme): An enum representing the theme to use.

`HideSidebar`

- **Description**: Hides the sidebar in the ScalarOptions.
- **Parameters**: None.

`UseSearchAsHotKey`

- **Description**: Sets the search hotkey for the ScalarOptions.
- **Parameters**:
  - `hotKey` (char): The character representing the hotkey for search.

`AddAdditionalItem`

- **Description**: Adds an additional item to the ScalarOptions configuration.
- **Parameters**:
  - `key` (string): The key of the additional item.
  - `value` (object): The value of the additional item.

These customization options provide developers with flexibility in configuring the Scalar library to suit their specific needs and preferences. Developers can utilize these options to enhance the functionality and appearance of their applications seamlessly.

## Compatibility

| AspNetCore.Scalar | Scalar |
|-------------------|:------:|
| 1.0.0             | 1.17.12 |
| 1.1.0             | 1.19.2 |
| 1.1.1             | 1.22.48 |
| 1.1.2             | 1.23.5 |
| 1.1.3             | 1.24.72 |
| 1.1.4             | 1.24.75 |
| 1.1.5             | 1.25.6 |
| 1.1.6             | 1.25.9 |
| 1.1.7             | 1.25.17 |
| 1.1.8             | 1.25.63 |

This project was based on [Swashbuckle Redoc](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/tree/master/src/Swashbuckle.AspNetCore.ReDoc).

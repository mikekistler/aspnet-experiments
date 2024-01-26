# ApiDescription

This project is a simple example of how to use the `ApiDescription` package to generate
an OpenAPI description for your API.

## How I built it

Start with a new web api project.

```bash
dotnet new webapi -o ApiDescription
dotnet sln add ApiDescription/ApiDescription.csproj
```

Personal preference: convert line endings to LF.

```bash
cd ApiDescription
find . -type f | grep -v 'obj/' | grep -v 'bin/' | grep -v .git | while read f; do sed -i '' 's/\r//' $f; done
```

Add the [`ApiDescription.Server`] package.

```bash
dotnet add package Microsoft.Extensions.ApiDescription.Server --version 8.0.1
```

and enable it in the project file.

```xml
  <PropertyGroup>
    <OpenApiGenerateDocumentsOnBuild>true</OpenApiGenerateDocumentsOnBuild>
  </PropertyGroup>
```

With these changes, `dotnet build` will generate an OpenAPI v3 description
for the service in `obj/API/ApiDescription.json`.

## Viewing the generated OpenAPI description in VSCode

Now that we are generating the OpenAPI description, we can use the [OpenAPI (Swagger) Editor] VSCode extension
to view SwaggerUI or ReDoc documentation based on this description.

But to get the "Try it out" functionality, we need to add the `servers` section to the OpenAPI description
and we need to add CORS support to the API.

### Adding the `servers` section

The easiest way to add the `servers` section is with the `AddServer` extension method of `SwaggerGenOptions`.

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.AddServer(new OpenApiServer { Url = "http://localhost:5252", Description = "Localhost" });
});
```

I hard-coded the port number, but it would be better to get it from the configuration.

### Adding CORS support

To add CORS support, we need to add the `CorsPolicy` to the service collection.

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("vscode-webview://1f6ukv751kufrd8aitq58rbkqn8scflggesvhor0jlnh4gh96mac")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

The origin is the URI of the Swagger UI webview. I got this by running a local `nc` server and
capturing the request.

```text
>nc -l 5252
GET /weatherforecast HTTP/1.1
Host: localhost:5252
Connection: keep-alive
sec-ch-ua: "Not.A/Brand";v="8", "Chromium";v="114"
accept: application/json
sec-ch-ua-mobile: ?0
User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Code/1.85.2 Chrome/114.0.5735.289 Electron/25.9.7 Safari/537.36
sec-ch-ua-platform: "macOS"
Origin: vscode-webview://1f6ukv751kufrd8aitq58rbkqn8scflggesvhor0jlnh4gh96mac
Sec-Fetch-Site: cross-site
Sec-Fetch-Mode: cors
Sec-Fetch-Dest: empty
Accept-Encoding: gzip, deflate, br
Accept-Language: en-US
```

with these changes in place, we can now run the API, view the documentation in the Swagger UI webview,
and use the "Try it out" functionality.

![Swagger UI](./images/swagger-ui.png)

## References

- [`ApiDescription.Server`]
- [OpenAPI (Swagger) Editor]
- [API Info and Description]
-
<!-- Links -->

[`ApiDescription.Server`]: https://www.nuget.org/packages/Microsoft.Extensions.ApiDescription.Server
[OpenAPI (Swagger) Editor]: https://marketplace.visualstudio.com/items?itemName=42Crunch.vscode-openapi
[API Info and Description]: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=netcore-cli#api-info-and-description
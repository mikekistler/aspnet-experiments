using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Update the default JsonOptions to skip serializing nulls and use RFC 3339 format for date-times
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
    options.SerializerOptions.Converters.Add(new DateTimeJsonConverter());
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/test1", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.UtcNow.AddDays(index),
            Random.Shared.Next(-20, 55),
            index % 2 == 0 ? summaries[Random.Shared.Next(summaries.Length)] : null
        ))
        .ToArray();
    return forecast;
})
.WithName("test1")
.WithOpenApi();


app.MapPost("/test2", ([FromBody] Test2Request request) =>
{
    return new
    {
        Name = request.Name,
        Age = request.Age,
        City = request.City
    };
})
.WithName("test2")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateTimeOffset Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal class Test2Request
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? City { get; set; }
}

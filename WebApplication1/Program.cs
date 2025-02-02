var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// This code defines an HTTP GET endpoint for "/weatherforecast"
app.MapGet("/weatherforecast", () =>
{
    // Generate a forecast for the next 5 days
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            // Set the date for each forecast day
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            // Generate a random temperature between -20 and 55 degrees Celsius
            Random.Shared.Next(-20, 55),
            // Select a random weather summary from the predefined array
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray(); // Convert the forecast to an array
    return forecast; // Return the generated forecast
})
.WithName("GetWeatherForecast") // Assign a name to the endpoint for documentation
.WithOpenApi(); // Enable OpenAPI (Swagger) support for this endpoint

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// Create a new WebApplication builder instance with default configuration
var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container
// These services are used for API documentation and exploration
builder.Services.AddEndpointsApiExplorer();  // Adds API explorer capabilities for endpoint metadata
builder.Services.AddSwaggerGen();            // Adds Swagger/OpenAPI generation services

// Build the WebApplication instance with all configured services
var app = builder.Build();

// Configure middleware pipeline based on environment
if (app.Environment.IsDevelopment())  // Check if running in development environment
{
    app.UseSwagger();     // Enable Swagger JSON endpoint
    app.UseSwaggerUI();   // Enable Swagger UI web interface
}

// Add middleware to redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Define an array of weather descriptions
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", 
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// Define a GET endpoint at "/weatherforecast" path
app.MapGet("/weatherforecast", () =>
{
    // Generate weather forecasts for 5 days
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            // Create date for each forecast, starting from tomorrow
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            // Generate random temperature between -20°C and 55°C
            Random.Shared.Next(-20, 55),
            // Pick a random weather description from summaries array
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();  // Convert IEnumerable to array
    return forecast; // Return the array of forecasts
})
.WithName("GetWeatherForecast")  // Set operation name for OpenAPI documentation
.WithOpenApi();                  // Enable OpenAPI/Swagger support for this endpoint

// Start the application
app.Run();

// Define WeatherForecast record (immutable data type)
record WeatherForecast(
    DateOnly Date,           // Date of the forecast
    int TemperatureC,       // Temperature in Celsius
    string? Summary         // Optional weather description
)
{
    // Calculated property that converts Celsius to Fahrenheit
    // Formula: °F = (°C * 9/5) + 32
    // Note: Using simplified integer arithmetic with 0.5556 ≈ 5/9
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

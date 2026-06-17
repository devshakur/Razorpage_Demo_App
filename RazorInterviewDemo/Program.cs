using RazorInterviewDemo.Services.Weather;
using RazorInterviewDemo.Services.Weather.OpenMeteo;

var builder = WebApplication.CreateBuilder(args);

var applicationUrls = builder.Configuration["ASPNETCORE_URLS"]
    ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
    ?? string.Empty;
var hasHttpsEndpoint = applicationUrls.Contains("https://", StringComparison.OrdinalIgnoreCase);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<IOpenMeteoClient, OpenMeteoClient>(client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient("Nominatim", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.UserAgent.ParseAdd("RazorInterviewDemo/1.0");
});
builder.Services.AddScoped<IWeatherDashboardService, WeatherDashboardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("An unexpected error occurred.");
        });
    });
    app.UseHsts();
}

if (hasHttpsEndpoint)
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapGet("/Weather", (HttpRequest request) =>
    Results.Redirect($"/{request.QueryString}"));
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

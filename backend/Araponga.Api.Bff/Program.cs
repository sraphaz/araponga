using Araponga.Bff.Middleware;
using Araponga.Bff.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BffOptions>(builder.Configuration.GetSection(BffOptions.SectionName));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IJourneyResponseCache, JourneyResponseCache>();

builder.Services.AddHttpClient<IJourneyApiProxy, JourneyApiProxy>(client =>
{
    var baseUrl = builder.Configuration["Bff:ApiBaseUrl"]?.TrimEnd('/');
    if (!string.IsNullOrEmpty(baseUrl))
        client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Araponga BFF (Journeys)",
        Version = "v2",
        Description = "Backend for Frontend - Jornadas (onboarding, feed, eventos). Aplicação separada que encaminha para a API principal."
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "BFF Journeys v2");
});

app.UseHttpsRedirection();
app.UseMiddleware<JourneyProxyMiddleware>();
app.MapControllers();

app.Run();

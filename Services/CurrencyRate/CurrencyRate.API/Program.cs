using MCF.Core;
using MCF.CurrencyRate.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<AppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddServiceDefaults();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UsePathBase("/api/currency-rate");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
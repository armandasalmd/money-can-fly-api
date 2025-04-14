var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CurrencyRate_API>("currency-rate");
builder.AddProject<Projects.FinanceMaster_API>("finance-master");
builder.AddProject<Projects.Identity_API>("identity");
builder.AddProject<Projects.Import_API>("import");
builder.AddProject<Projects.Prediction_API>("prediction");

builder.Build().Run();
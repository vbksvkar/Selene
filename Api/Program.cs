using Api.Infrastructure;
using BusinessLogic.Extensions;
using Clients.Extensions;
using DataStore.Extensions;
using Models.Configuration;
using Models.Extensions;
using Serilog;
using Worker.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<SeleneConfig>();

builder.Host.UseSerilog((builder, logger) => {
    logger.ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterClients();
builder.Services.RegisterDataStore();
builder.RegisterModels(builder.Services);
builder.Services.RegisterWorker();
builder.Services.RegisterBusinessLogic();
builder.Services.AddControllers();
builder.Services.AddSingleton<IUnhandledExceptionHandler, UnhandledExceptionHandler>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

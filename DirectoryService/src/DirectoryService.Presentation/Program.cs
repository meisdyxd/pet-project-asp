using DirectoryService.Application;
using DirectoryService.Application.Interfaces;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Presentation.RegisterServices;
using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Database.Dapper;
using DirectoryService.Presentation;
using DirectoryService.Presentation.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services
    .SwaggerConfigure()
    .AddInfrastructure(configuration)
    .AddApplication()
    .ConfigureSerilog(configuration)
    .AddScoped<CustomExceptionMiddleware>()
    .AddSingleton<IDapperConnectionFactory, DapperConnectionFactory>()
    .AddControllers();

var app = builder.Build();

app.UseCustomExceptionMiddleware();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();
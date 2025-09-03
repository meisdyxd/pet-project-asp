using DirectoryService.Application;
using DirectoryService.Presentation.RegisterServices;
using DirectoryService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services
    .SwaggerConfigure()
    .AddInfrastructure(configuration)
    .AddApplication()
    .AddControllers();

var app = builder.Build();

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
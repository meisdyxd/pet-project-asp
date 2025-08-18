using DirectoryService.Presentation.RegisterServices;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services
    .SwaggerConfigure()
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
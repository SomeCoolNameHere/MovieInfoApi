using MovieInfoApi.Extentions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddSwaggerConfiguration();
services.ConfigureMovieServices(configuration);

services.AddEndpointsApiExplorer();
services.AddControllers();
services.AddAutomapperConfiguration();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase("/swagger");
app.MapControllers();
app.UseRouting();
app.Run();
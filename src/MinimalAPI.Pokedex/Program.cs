using Microsoft.OpenApi.Models;
using MinimalAPI.Pokedex;
using MinimalAPI.Pokedex.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo()
{
    Description = "Pokedex API with .NET 6 Minimal API and MediatR",
    Title = "Pokedex API",
    Version = "v2",
    Contact = new OpenApiContact()
    {
        Name = "Colin Smith",
        Url = new Uri("https://github.com/colinmxs")
    }
}));
builder.Services.AddCors();
builder.Services.AddScoped<IPokedexRepository, JsonFilePokedexRepository>();
builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();

app.UseCors(opts => opts.AllowAnyOrigin());
app.UseStaticFiles();
app.UseSwagger();
app.RegisterRoutes();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokedex API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.Run();
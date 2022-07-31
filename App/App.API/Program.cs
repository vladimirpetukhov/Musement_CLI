using App.Infrastrucure;
using App.Services;
using App.Core.Models;
using App.Services.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/cities", async (TUIClient client) =>
{
    IEnumerable<City> cities = default;
    try
    {
        cities = await client.GetSities();
    }
    catch (global::System.Exception)
    {
        Results.BadRequest();
    }

    return cities;
}).Produces(400).Produces(200);

app.MapGet("/cities/{id}", async (int id, TUIClient client) =>
 {
     City city = default;
     try
     {
         city = await client.GetSities(id);

         if (city == null)
         {
             Results.NotFound(id);
         }
     }
     catch (global::System.Exception)
     {
         Results.BadRequest();
     }

     return city;
 }).Produces(400).Produces(404).Produces(200);

app.Run();


using Microsoft.OpenApi.Models;
using Zoo.Application.Interfaces;
using Zoo.Application.Services;
using Zoo.Domain.Interfaces;
using Zoo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zoo Management API", Version = "v1" });
});

// Register repositories
builder.Services.AddSingleton<IAnimalRepository, AnimalRepository>();
builder.Services.AddSingleton<IEnclosureRepository, EnclosureRepository>();
builder.Services.AddSingleton<IFeedingScheduleRepository, FeedingScheduleRepository>();

// Register specialized services
builder.Services.AddScoped<AnimalTransferService>();
builder.Services.AddScoped<FeedingOrganizationService>();
builder.Services.AddScoped<ZooStatisticsService>();

// Register application services
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IEnclosureService, EnclosureService>();
builder.Services.AddScoped<IFeedingScheduleService, FeedingScheduleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo Management API v1"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

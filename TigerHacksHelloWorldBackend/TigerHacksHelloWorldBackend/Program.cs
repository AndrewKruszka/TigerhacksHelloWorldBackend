using Geohash;
using Library.DataAccess;
using Library.DataAccess.Interfaces;
using Library.Repositories;
using Library.Utilities;
using Library.Utilities.Interfaces;
using Library.Workers;
using Library.Workers.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache(x => new MemoryCacheOptions().SizeLimit = 1024);
builder.Services.AddScoped<ICacheHelper, CacheHelper>();
builder.Services.AddScoped<ISettings, Settings>();
builder.Services.AddScoped<Geohasher>();


builder.Services.AddScoped<IInteractionDataAccess, InteractionDataAccess>();
builder.Services.AddScoped<ILandmarkDataAccess, LandmarkDataAccess>();
builder.Services.AddScoped<IScrapbookDataAccess, ScrapbookDataAccess>();
builder.Services.AddScoped<IUserDataAccess, UserDataAccess>();
builder.Services.AddScoped<IInteractionWorker, InteractionWorker>();
builder.Services.AddScoped<IUserWorker, UserWorker>();
builder.Services.AddScoped<IGridWorker, GridWorker>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();

app.Run();


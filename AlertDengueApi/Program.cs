using AlertDengueApi.Data;
using AlertDengueApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using AlertDengueApi.Repositories;
using AlertDengueApi.Services;
using AlertDengueApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

// CORS para permitir chamadas do Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IDengueAlertRepository, DengueAlertRepository>();
builder.Services.AddScoped<IDengueAlertService, DengueAlertService>();
builder.Services.AddScoped<IEpidemologicalWeekHelper, EpidemologicalWeekHelper>();
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProleitPocBackend.Data;
using ProleitPocBackend.Hubs;
using ProleitPocBackend.IRepository;
using ProleitPocBackend.Repository;
using ProleitPOCBackend.IService;
using ProleitPOCBackend.Service;

var builder = WebApplication.CreateBuilder(args);

// Configure Database Connection
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");

// Configure Entity Framework
builder.Services.AddDbContext<ProleitPocBackendDbContext>(option =>
    option.UseSqlServer(connectionString, b => b.MigrationsAssembly("ProleitPocBackend.API"))
);

// Configure CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "http://localhost:4201"
            ) // Allow requests from this origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow credentials (cookies, authorization headers)
    });
});

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceService, DeviceService>();

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Returns API versions in response headers
    options.AssumeDefaultVersionWhenUnspecified = true; // Uses the default version if not specified
    options.DefaultApiVersion = new ApiVersion(1, 0); // Set default version to 1.0
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Read version from URL
});

// Add SignalR
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ProleitPocBackendHubs>("/dataHub");
app.UseHttpsRedirection();

app.Run();
using Microsoft.OpenApi.Models;
using project.Interfaces;
using project.middleware;
using project.Models;
using project.Services;
using Serilog;
using Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "Logs/{Year}/{Month}/{Day}/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Host.UseSerilog(); // הוספת Serilog

// הוסף את השירותים שלך כאן
//var app = builder.Build();

// LoggerSetup.ConfigureLogger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookApi", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                new string[] { }
            },
        }
    );
});

// הוספת שירותי CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

// הוספת שירותים לקונטיינר.

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddService();
builder.Services.AddCustomAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseErrorMiddleware();
app.UseAuthMiddleware<Author>();
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // הוספת המידלוויר של CORS
app.UseAuthorization();
app.MapControllers();

app.MapGet(
    "/",
    async context =>
    {
        context.Response.Redirect("/login");
    }
);

app.MapGet(
    "/login",
    async context =>
    {
        context.Response.ContentType = "text/html";
        await context.Response.SendFileAsync(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "login.html")
        );
    }
);
app.MapGet(
    "/{*page}",
    async context =>
    {
        var page = context.Request.RouteValues["page"]?.ToString() ?? "index"; // ברירת מחדל לעמוד index
        context.Response.ContentType = "text/html";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", $"{page}.html");

        if (File.Exists(filePath))
        {
            await context.Response.SendFileAsync(filePath);
        }
        else
        {
            context.Response.StatusCode = 404; // לא נמצא
        }
    }
);

app.Run();

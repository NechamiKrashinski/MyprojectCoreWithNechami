using Microsoft.OpenApi.Models;
using project.Interfaces;
using project.middleware;
using project.Models;
using project.Services;
using Serilog;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Debug()
//     .WriteTo.Console()
//     .WriteTo.File(
//         "logs/.log",
//         rollingInterval: RollingInterval.Day,
//         fileSizeLimitBytes: 100_000_000,
//         retainedFileCountLimit: 30
//     )
//     .CreateLogger();

// builder.Host.UseSerilog();

// Swagger & Authentication setup
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

builder.Services.AddControllers();
builder.Services.AddService();
builder.Services.AddCustomAuthentication(builder.Configuration);

var app = builder.Build();

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// טעינת קבצים סטטיים - חייב להיות לפני ראוטים!
app.UseDefaultFiles();
app.UseStaticFiles();

// שאר מידלוורים
//app.UseMiddleware<RequestLoggingMiddleware>();
app.UseErrorMiddleware();
app.UseAuthMiddleware<Author>();
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();

app.MapGet(
    "/",
    context =>
    {
        context.Response.Redirect("author.html");
        return Task.CompletedTask;
    }
);

app.MapFallback(context =>
{
    context.Response.StatusCode = 404;
    return context.Response.WriteAsync("Not found");
});

app.Run();

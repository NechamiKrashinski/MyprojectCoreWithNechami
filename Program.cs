using Microsoft.OpenApi.Models;
using project.middleware;
using project.middlewares;
using project.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
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
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            
            .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Hello, world!");
      await Log.CloseAndFlushAsync();
        
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


 app.UseHttpsRedirection();



app.UseLogMiddleware();
//app.UseErrorMiddleware();
app.UseStaticFiles(); 
app.UseRouting();
//app.UseAuthMiddleware(); // מיקום זה חשוב כדי שיתבצע על כל בקשה
app.UseAuthentication();
app.UseAuthorization();
//app.UseDefaultFiles();
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "login.html" }
});

app.UseUserMiddleware();
app.MapControllers();

app.Run();

// app.UseLogMiddleware();
// //app.UseErrorMiddleware();

// app.UseStaticFiles(); 

// app.UseRouting();
//  app.UseAuthMiddleware();
// app.UseDefaultFiles(new DefaultFilesOptions
// {
//     DefaultFileNames = new List<string> { "book.html" }
// });

//  app.UseUserMiddleware();
// app.UseAuthentication();
// app.UseAuthorization();
// app.MapControllers();

// app.Run();

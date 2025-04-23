using Microsoft.OpenApi.Models;
using project.middleware;
using project.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1",new OpenApiInfo{Title="BOOK API",Version="v1"});
    c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
        In =ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSservic();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseLogMiddleware();
app.UseErrorMiddleware();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
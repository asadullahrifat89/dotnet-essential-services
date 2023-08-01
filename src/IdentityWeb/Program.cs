using IdentityCore.Contracts.Declarations.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using IdentityCore.Extensions;
using Microsoft.OpenApi.Models;
using IdentityCore;
using Serilog;
using IdentityCore.Contracts.Implementations.Services;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.Request;
});

#endif

builder.Services.AddCors(options =>
{

#if DEBUG
    options.AddPolicy("CorsPolicy", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
#else
    options.AddPolicy("CorsPolicy", policy => { policy.WithOrigins().SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader().AllowAnyMethod(); });
#endif

});

// Add http context accessor
builder.Services.AddHttpContextAccessor();

// Add mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateUserCommand).GetTypeInfo().Assembly));

// Add validators
builder.Services.AddValidators();

// Add services
builder.Services.AddCoreServices();

// Add repositories
builder.Services.AddRepositories();

builder.Services.AddMvc();

// Add services to the container.
builder.Services.AddControllers();

var environemntVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (environemntVariable != null && Constants.AllowedSwaggerEnvironments.Contains(environemntVariable))
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service Web Api", Version = "v1" });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
}

// Add serilog
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();

#if !DEBUG
builder.Logging.ClearProviders();
#endif

builder.Logging.AddSerilog(logger);

// App build
var app = builder.Build();

if (app.Environment.IsDevelopment() || Constants.AllowedSwaggerEnvironments.Contains(environemntVariable))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#if DEBUG
app.UseHttpLogging();
#endif

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseMiddleware<JwtMiddleware>(); // custom jwt auth middleware

app.MapControllers();

app.Run();
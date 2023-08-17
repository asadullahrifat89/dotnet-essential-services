using System.Reflection;
using Base.Application.Providers.Interfaces;
using Base.Application.Extensions;
using Base.Shared.Constants;
using Blob.Application.Commands;
using Blob.Application.Commands.Validators;
using Blob.Domain.Repositories.Interfaces;
using Email.Application.Commands;
using Email.Application.Commands.Validators;
using Email.Application.Services;
using Email.Application.Services.Interfaces;
using Email.Domain.Repositories.Interfaces;
using Identity.Application.Commands;
using Identity.Application.Commands.Validators;
using Identity.Application.Middlewares;
using Identity.Application.Providers.Interfaces;
using Identity.Application.Services.Interfaces;
using Identity.Domain.Repositories.Interfaces;
using Language.Application.Commands;
using Language.Application.Commands.Validators;
using Language.Domain.Repositories.Interfaces;
using Microsoft.OpenApi.Models;
using Serilog;

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
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AuthenticateCommand).GetTypeInfo().Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UploadBlobFileCommand).GetTypeInfo().Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateEmailTemplateCommand).GetTypeInfo().Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddLingoAppCommand).GetTypeInfo().Assembly));

// teams
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddProductSearchCriteriaCommand).GetTypeInfo().Assembly));

// Add validators
builder.Services.AddValidators<AuthenticateCommandValidator>();
builder.Services.AddValidators<UploadBlobFileCommandValidator>();
builder.Services.AddValidators<CreateEmailTemplateCommandValidator>();
builder.Services.AddValidators<AddLingoAppCommandValidator>();

// teams
//builder.Services.AddValidators<AddProductSearchCriteriaCommandValidator>();


// Add services
builder.Services.AddServices<IJwtService>();
builder.Services.AddServices<IEmailSenderService>();

builder.Services.AddHostedService<EmailSenderHostedService>();

// Add providers
builder.Services.AddProviders<IMongoDbContextProvider>();
builder.Services.AddProviders<IAuthenticationContextProvider>();

// Add repositories
builder.Services.AddRepositories<IAuthTokenRepository>();
builder.Services.AddRepositories<IBlobFileRepository>();
builder.Services.AddRepositories<IEmailTemplateRepository>();
builder.Services.AddRepositories<ILanguageResourcesRepository>();

// teams
//builder.Services.AddRepositories<IProductSearchCriteriaRepository>();

builder.Services.AddMvc();

// Add services to the container.
builder.Services.AddControllers();

var environemntVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (environemntVariable != null && CommonConstants.AllowedSwaggerEnvironments.Contains(environemntVariable))
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Essential Web Service", Version = "v1" });
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
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
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

if (app.Environment.IsDevelopment() || CommonConstants.AllowedSwaggerEnvironments.Contains(environemntVariable))
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
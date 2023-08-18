using System.Reflection;
using Base.Application.Extensions;
using Base.Shared.Constants;
using Blob.Application.Commands;
using Blob.Application.Commands.Validators;
using Email.Application.Commands;
using Email.Application.Commands.Validators;
using Email.Application.Services;
using Identity.Application.Commands;
using Identity.Application.Commands.Validators;
using Identity.Application.Middlewares;
using Language.Application.Commands;
using Language.Application.Commands.Validators;
using Microsoft.OpenApi.Models;
using Serilog;
using Identity.Application.Services;
using Base.Application.Providers;
using Identity.Application.Providers;
using Identity.Infrastructure.Persistence;
using Blob.Infrastructure.Persistence;
using Email.Infrastructure.Persistence;
using Language.Infrastructure.Persistence;
using Teams.ContentMangement.Infrastructure.Persistence;
using Teams.ContentMangement.Application.Commands;
using Teams.ContentMangement.Application.Commands.Validators;
using Base.Application.Attributes;
using Teams.CustomerEngagement.Application.Commands;
using Teams.CustomerEngagement.Application.Commands.Validators;
using Teams.CustomerEngagement.Infrastructure.Persistence;

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

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddProductSearchCriteriaCommand).GetTypeInfo().Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddQuotationCommand).GetTypeInfo().Assembly));

// Add validators
builder.Services.AddValidators<AuthenticateCommandValidator>();
builder.Services.AddValidators<UploadBlobFileCommandValidator>();
builder.Services.AddValidators<CreateEmailTemplateCommandValidator>();
builder.Services.AddValidators<AddLingoAppCommandValidator>();

builder.Services.AddValidators<AddProductSearchCriteriaCommandValidator>();
builder.Services.AddValidators<AddQuotationCommandValidator>();

// Add services
builder.Services.AddServices<JwtService>();
builder.Services.AddServices<EmailSenderService>();

builder.Services.AddHostedService<EmailSenderHostedService>();

// Add providers
builder.Services.AddProviders<MongoDbContextProvider>();
builder.Services.AddProviders<AuthenticationContextProvider>();

// Add repositories
builder.Services.AddRepositories<AuthTokenRepository>();
builder.Services.AddRepositories<BlobFileRepository>();
builder.Services.AddRepositories<EmailTemplateRepository>();
builder.Services.AddRepositories<LanguageResourcesRepository>();

builder.Services.AddRepositories<ProductSearchCriteriaRepository>();
builder.Services.AddRepositories<QuotationRepository>();

builder.Services.AddMvc(mvc =>
{
    mvc.Conventions.Add(new ControllerNameAttributeConvention());
});

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
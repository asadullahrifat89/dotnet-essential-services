using IdentityCore.Contracts.Declarations.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using FluentValidation;
using IdentityCore.Contracts.Implementations.Commands.Validators;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Contracts.Implementations.Services;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Implementations.Repositories;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.Request;
});

#endif

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// Add http context accessor
builder.Services.AddHttpContextAccessor();

// Add mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateUserCommand).GetTypeInfo().Assembly));

// Add validators
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters().AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

// Add services
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();

// Add repositories
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddMvc();

// Add services to the container.
builder.Services.AddControllers();


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

#if DEBUG

app.UseHttpLogging();

#endif

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
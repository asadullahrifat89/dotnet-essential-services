using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Contracts.Implementations.Commands.Validators;
using IdentityCore.Contracts.Implementations.Repositories;
using IdentityCore.Contracts.Implementations.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdentityCore.Extensions
{

    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationClientsideAdapters().AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
            return services;
        }

        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuthenticationContextProvider, AuthenticationContextProvider>();

            var allServices = Assembly.GetAssembly(typeof(MongoDbService))?.GetTypes().Where(type => !type.IsInterface && type.Name.EndsWith("Service"));

            if (allServices is not null)
            {
                foreach (var item in allServices)
                {
                    Type serviceType = item.GetTypeInfo().ImplementedInterfaces.First();
                    services.AddSingleton(serviceType, item);
                }
            }


            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var allRepositories = Assembly.GetAssembly(typeof(UserRepository))?.GetTypes().Where(type => !type.IsInterface && type.Name.EndsWith("Repository"));

            if (allRepositories is not null)
            {
                foreach (var item in allRepositories)
                {
                    Type serviceType = item.GetTypeInfo().ImplementedInterfaces.First();
                    services.AddSingleton(serviceType, item);
                }
            }

            return services;
        }
    }
}

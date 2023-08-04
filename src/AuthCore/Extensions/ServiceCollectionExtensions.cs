using BaseCore.Declarations.Services;
using BaseCore.Implementations.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BaseCore.Extensions
{

    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddValidators<T>(this IServiceCollection services)
        {
            services.AddFluentValidationClientsideAdapters().AddValidatorsFromAssemblyContaining<T>();
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

        public static IServiceCollection AddRepositories<T>(this IServiceCollection services)
        {
            var allRepositories = Assembly.GetAssembly(typeof(T))?.GetTypes().Where(type => !type.IsInterface && type.Name.EndsWith("Repository"));

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

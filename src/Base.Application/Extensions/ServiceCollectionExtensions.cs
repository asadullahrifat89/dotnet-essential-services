using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Base.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValidators<T>(this IServiceCollection services)
        {
            services.AddFluentValidationClientsideAdapters().AddValidatorsFromAssemblyContaining<T>();
            return services;
        }

        public static IServiceCollection AddServices<T>(this IServiceCollection services)
        {
            AddServiceFromAssemblyWithKeyword<T>(services, "Service");
            return services;
        }

        public static IServiceCollection AddProviders<T>(this IServiceCollection services)
        {
            AddServiceFromAssemblyWithKeyword<T>(services, "Provider");
            return services;
        }

        public static IServiceCollection AddRepositories<T>(this IServiceCollection services)
        {
            AddServiceFromAssemblyWithKeyword<T>(services, "Repository");
            return services;
        }

        private static void AddServiceFromAssemblyWithKeyword<T>(IServiceCollection services, string keyword)
        {
            var foundServices = Assembly.GetAssembly(typeof(T))?.GetTypes().Where(type => !type.IsInterface && type.Name.EndsWith(keyword));

            if (foundServices is not null)
            {
                foreach (var service in foundServices)
                {
                    Type implementedInterface = service.GetTypeInfo().ImplementedInterfaces.First();

                    if (implementedInterface is not null)
                        services.AddSingleton(implementedInterface, service);
                }
            }
        }
    }
}

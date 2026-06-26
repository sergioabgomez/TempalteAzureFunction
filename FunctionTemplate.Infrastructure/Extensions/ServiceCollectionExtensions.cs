using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionTemplate.Infrastructure.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddRegisterService<TBaseInterface>(this IServiceCollection services, Assembly[]? assemblies = null, ServiceLifetime lifetime = ServiceLifetime.Scoped)
		{
			Type baseType = typeof(TBaseInterface);
			assemblies ??= AppDomain.CurrentDomain.GetAssemblies();

			IEnumerable<Type> allTypes = assemblies.Where(assemby => !assemby.IsDynamic).SelectMany(assemby => assemby.GetTypes())
							.Where(types => types.IsClass && !types.IsAbstract && baseType.IsAssignableFrom(types));

			foreach (Type implementationType in allTypes)
			{
				IEnumerable<Type> interfaces = implementationType.GetInterfaces()
					.Where(@interface => baseType.IsAssignableFrom(@interface) && @interface != baseType);

				foreach (Type @interface in interfaces)
				{
					services.Add(new ServiceDescriptor(@interface, implementationType, lifetime));
				}
			}

			return services;
		}
	}
}

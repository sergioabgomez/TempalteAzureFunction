using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunctionTemplate.Host.Installers.Contracts;

namespace FunctionTemplate.Host.Installers.Extensions
{
	/// <summary>
	/// Extension methods for installers. These methods will be called at startup to execute all installers in the assembly.
	/// </summary>
	public static class InstallerExtensions
	{
		/// <summary>
		/// Installs all services in the assembly that implement IInstallerServiceCollection. This method will be called at startup to execute all installers in the assembly.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
		{
			var installers = typeof(Program).Assembly.ExportedTypes.Where(type =>
				typeof(IInstallerServiceCollection).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).Select(Activator.CreateInstance).Cast<IInstallerServiceCollection>().ToList();

			installers.ForEach(installer => installer.InstallServices(services, configuration));
		}

		/// <summary>
		/// Installs all services in the assembly that implement IInstallerApplicationBuilder. This method will be called at startup to execute all installers in the assembly.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="configuration"></param>
		public static void InstallApplicationInAssembly(this IApplicationBuilder app, IConfiguration configuration)
		{
			var installers = typeof(Program).Assembly.ExportedTypes.Where(type =>
				typeof(IInstallerApplicationBuilder).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).Select(Activator.CreateInstance).Cast<IInstallerApplicationBuilder>().ToList();

			installers.ForEach(installer => installer.InstallApplication(app, configuration));
		}
	}
}

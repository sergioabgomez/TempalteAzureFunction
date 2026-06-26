using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunctionTemplate.Application;
using FunctionTemplate.Infrastructure;
using FunctionTemplate.Infrastructure.Extensions;
using FunctionTemplate.Infrastructure.Services;
using FunctionTemplate.Host.Installers.Contracts;

namespace FunctionTemplate.Host.Installers
{
	/// <summary>
	/// Registers application and infrastructure services for dependency injection.
	/// </summary>
	public class ServicesInstaller : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			Assembly[] assemblies = new[] { typeof(DummyApplication), typeof(DummyInfrastructure) }.Select(assembly => assembly.Assembly).ToArray();

			services.AddRegisterService<IServiceScoped>(assemblies, ServiceLifetime.Scoped);
			services.AddRegisterService<IServiceTransient>(assemblies, ServiceLifetime.Transient);
			services.AddRegisterService<IServiceSingleton>(assemblies, ServiceLifetime.Singleton);
		}
	}
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionTemplate.Host.Installers.Contracts
{
	/// <summary>
	/// Contract for installers. Implement this interface to create an installer that will be automatically executed at startup.
	/// </summary>
	public interface IInstallerApplicationBuilder
	{
		/// <summary>
		/// Install application middlewares, endpoints, etc.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="configuration"></param>
		public void InstallApplication(IApplicationBuilder app, IConfiguration configuration);
	}

	/// <summary>
	/// Contract for installers. Implement this interface to create an installer that will be automatically executed at startup.
	/// </summary>
	public interface IInstallerServiceCollection
	{
		/// <summary>
		/// Install services in the DI container.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		public void InstallServices(IServiceCollection services, IConfiguration configuration);
	}
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunctionTemplate.Host.Installers.Contracts;
using StackExchange.Redis;

namespace FunctionTemplate.Host.Installers
{
	/// <summary>
	/// Installer for Cache services.
	/// </summary>
	public class InstallerCache : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddMemoryCache();

			// Distributed cache (Redis) can be configured here
		}
	}
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunctionTemplate.Host.Installers.Contracts;

namespace FunctionTemplate.Host.Installers
{
	/// <summary>
	/// Installer for HttpClient.
	/// </summary>
	public class InstalleHttpClient : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			// HTTP client registration goes here
		}
	}
}

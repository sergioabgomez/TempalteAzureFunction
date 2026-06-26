using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunctionTemplate.Host.Installers.Contracts;

namespace FunctionTemplate.Host.Installers
{
	/// <summary>
	/// Installer for options (IOptions pattern).
	/// </summary>
	public class InstallerOptions : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions();

			// Bind configuration sections to options classes here
		}
	}
}

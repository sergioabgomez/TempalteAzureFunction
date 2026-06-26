using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunctionTemplate.Host.Installers.Contracts;

namespace FunctionTemplate.Host.Installers
{
	/// <summary>
	/// Installer for Swagger/OpenAPI settings.
	/// </summary>
	public class SwaggerInstaller : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<OpenApiSettings>(o => o.HideSwaggerUI = true);
		}
	}
}

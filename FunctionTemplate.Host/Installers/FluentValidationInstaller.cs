using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunctionTemplate.Application;
using FunctionTemplate.Host.Installers.Contracts;

namespace FunctionTemplate.Host.Installers
{
	/// <summary>
	/// Installs FluentValidation validators from the assembly containing the DummyApplication class.
	/// </summary>
	public class FluentValidationInstaller : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddValidatorsFromAssemblyContaining<DummyApplication>(
			includeInternalTypes: true);
		}
	}
}

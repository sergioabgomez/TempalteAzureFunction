using Cortex.Mediator.Behaviors.FluentValidation.DependencyInjection;
using Cortex.Mediator.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunctionTemplate.Application;
using FunctionTemplate.Host.Installers.Contracts;

namespace FunctionTemplate.Host.Installers
{
	/// <summary>
	/// Installer for Mediator services.
	/// </summary>
	public class MediatorRegisterInstaller : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddCortexMediator(
			handlerAssemblyMarkerTypes: [typeof(DummyApplication)],
			configure: options =>
			{
				options.AddDefaultBehaviors();
				options.AddFluentValidationBehaviors();
			});
		}
	}
}

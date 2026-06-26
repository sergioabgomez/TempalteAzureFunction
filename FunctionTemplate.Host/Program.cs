using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using FunctionTemplate.Host.Installers.Extensions;
using FunctionTemplate.Host.Middlewares;

IHost host = new HostBuilder()
	.ConfigureFunctionsWebApplication(app =>
	{
		app.UseMiddleware<ExceptionHandlingMiddleware>();
	})
	.ConfigureOpenApi()
	.ConfigureServices((context, services) =>
	{
		services.InstallServicesInAssembly(context.Configuration);
	}).ConfigureLogging(logging =>
	{
		logging.AddConsole();
	})
	.Build();

await host.RunAsync();

using Cortex.Mediator;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FunctionTemplate.Application.Handlers.Commands.ProcessTimer;

namespace FunctionTemplate.Host.Functions;

public class SampleTimerFunction
{
	private readonly IMediator mediator;
	private readonly ILogger<SampleTimerFunction> logger;

	public SampleTimerFunction(IMediator mediator, ILogger<SampleTimerFunction> logger)
	{
		this.mediator = mediator;
		this.logger = logger;
	}

	[Function("SampleTimer")]
	public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo timer, FunctionContext context)
	{
		logger.LogInformation("SampleTimer function invoked at {Time}", DateTime.UtcNow);

		var command = new ProcessTimerCommand
		{
			ScheduledTime = timer.ScheduleStatus?.Next ?? DateTime.UtcNow
		};

		await mediator.SendCommandAsync(command);
	}
}

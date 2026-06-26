using Cortex.Mediator.Commands;
using Microsoft.Extensions.Logging;

namespace FunctionTemplate.Application.Handlers.Commands.ProcessTimer
{
	public sealed class ProcessTimerCommandHandler : ICommandHandler<ProcessTimerCommand>
	{
		private readonly ILogger<ProcessTimerCommandHandler> logger;

		public ProcessTimerCommandHandler(ILogger<ProcessTimerCommandHandler> logger)
		{
			this.logger = logger;
		}

		public Task Handle(ProcessTimerCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation(
				"Timer triggered at {Now}. Scheduled time was {Scheduled}.",
				DateTime.UtcNow, command.ScheduledTime);

			return Task.CompletedTask;
		}
	}
}

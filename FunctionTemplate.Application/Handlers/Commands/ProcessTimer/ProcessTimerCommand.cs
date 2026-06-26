using Cortex.Mediator.Commands;

namespace FunctionTemplate.Application.Handlers.Commands.ProcessTimer
{
	public class ProcessTimerCommand : ICommand
	{
		public DateTime ScheduledTime { get; init; }
	}
}

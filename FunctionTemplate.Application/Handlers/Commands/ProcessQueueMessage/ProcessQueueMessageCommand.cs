using Cortex.Mediator.Commands;

namespace FunctionTemplate.Application.Handlers.Commands.ProcessQueueMessage
{
	public class ProcessQueueMessageCommand : ICommand
	{
		public string MessageId { get; init; } = string.Empty;
		public string Content { get; init; } = string.Empty;
		public DateTime EnqueuedAt { get; init; }
	}
}

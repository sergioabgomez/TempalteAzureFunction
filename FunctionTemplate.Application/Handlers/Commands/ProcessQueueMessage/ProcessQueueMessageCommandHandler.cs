using Cortex.Mediator.Commands;
using Microsoft.Extensions.Logging;

namespace FunctionTemplate.Application.Handlers.Commands.ProcessQueueMessage
{
	public sealed class ProcessQueueMessageCommandHandler : ICommandHandler<ProcessQueueMessageCommand>
	{
		private readonly ILogger<ProcessQueueMessageCommandHandler> logger;

		public ProcessQueueMessageCommandHandler(ILogger<ProcessQueueMessageCommandHandler> logger)
		{
			this.logger = logger;
		}

		public Task Handle(ProcessQueueMessageCommand command, CancellationToken cancellationToken)
		{
			logger.LogInformation(
				"Queue message {MessageId} processed. Content: {Content}. Enqueued at: {EnqueuedAt}.",
				command.MessageId, command.Content, command.EnqueuedAt);

			return Task.CompletedTask;
		}
	}
}

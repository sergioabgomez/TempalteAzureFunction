using Cortex.Mediator;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FunctionTemplate.Application.Handlers.Commands.ProcessQueueMessage;

namespace FunctionTemplate.Host.Functions;

public class SampleQueueFunction
{
	private readonly IMediator mediator;
	private readonly ILogger<SampleQueueFunction> logger;

	public SampleQueueFunction(IMediator mediator, ILogger<SampleQueueFunction> logger)
	{
		this.mediator = mediator;
		this.logger = logger;
	}

	[Function("SampleQueue")]
	public async Task Run([QueueTrigger("sample-queue", Connection = "AzureWebJobsStorage")] string message, FunctionContext context)
	{
		logger.LogInformation("SampleQueue function triggered with message: {Message}", message);

		ProcessQueueMessageCommand command;

		try
		{
			command = JsonConvert.DeserializeObject<ProcessQueueMessageCommand>(message)
				?? throw new InvalidOperationException("Deserialized message is null.");
		}
		catch (JsonException ex)
		{
			logger.LogError(ex, "Failed to deserialize queue message: {Message}", message);
			command = new ProcessQueueMessageCommand
			{
				MessageId = Guid.NewGuid().ToString(),
				Content = message,
				EnqueuedAt = DateTime.UtcNow
			};
		}

		await mediator.SendCommandAsync(command);
	}
}

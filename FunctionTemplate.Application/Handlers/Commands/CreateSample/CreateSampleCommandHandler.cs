using Cortex.Mediator.Commands;
using FunctionTemplate.Application.Models.Responses;

namespace FunctionTemplate.Application.Handlers.Commands.CreateSample
{
	public sealed class CreateSampleCommandHandler : ICommandHandler<CreateSampleCommand, CreateSampleResponse>
	{
		public Task<CreateSampleResponse> Handle(CreateSampleCommand command, CancellationToken cancellationToken)
		{
			var response = new CreateSampleResponse
			{
				Id = Guid.NewGuid(),
				Name = command.Name,
				Description = command.Description,
				CreatedAt = DateTime.UtcNow
			};

			return Task.FromResult(response);
		}
	}
}

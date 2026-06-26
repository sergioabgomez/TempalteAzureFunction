using Cortex.Mediator.Commands;
using FunctionTemplate.Application.Models.Responses;
using FunctionTemplate.Infrastructure.Attributes;

namespace FunctionTemplate.Application.Handlers.Commands.CreateSample
{
	public class CreateSampleCommand : ICommand<CreateSampleResponse>
	{
		[FromBodyJson]
		public string Name { get; init; } = string.Empty;

		[FromBodyJson]
		public string? Description { get; init; }
	}
}

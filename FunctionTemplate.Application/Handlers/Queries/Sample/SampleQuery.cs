using Cortex.Mediator.Queries;
using FunctionTemplate.Application.Models.Responses;
using FunctionTemplate.Infrastructure.Attributes;

namespace FunctionTemplate.Application.Handlers.Queries.Sample
{
	public class SampleQuery : IQuery<SampleResponse>
	{
		[FromQuery]
		public string Name { get; init; } = string.Empty;

		[FromQuery]
		public bool? Uppercase { get; init; }
	}
}

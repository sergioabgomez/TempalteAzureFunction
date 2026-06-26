using Cortex.Mediator.Queries;
using FunctionTemplate.Application.Models.Responses;

namespace FunctionTemplate.Application.Handlers.Queries.Sample
{
	public sealed class SampleQueryHandler : IQueryHandler<SampleQuery, SampleResponse>
	{
		public Task<SampleResponse> Handle(SampleQuery query, CancellationToken cancellationToken)
		{
			var message = $"Hello, {query.Name}!";

			if (query.Uppercase == true)
				message = message.ToUpperInvariant();

			return Task.FromResult(new SampleResponse { Message = message });
		}
	}
}

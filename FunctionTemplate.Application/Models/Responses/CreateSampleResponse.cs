namespace FunctionTemplate.Application.Models.Responses
{
	public sealed class CreateSampleResponse
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string? Description { get; init; }
		public DateTime CreatedAt { get; init; }
	}
}

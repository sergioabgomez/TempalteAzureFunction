namespace FunctionTemplate.Application.Models.Requests
{
	public class CreateSampleRequest
	{
		public string Name { get; init; } = string.Empty;
		public string? Description { get; init; }
	}
}

namespace FunctionTemplate.Infrastructure.Attributes
{
	/// <summary>
	/// Marks a property to be bound from the request body (JSON) only.
	/// Properties without this attribute fall back to the query string
	/// when no body is present.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class FromBodyJsonAttribute : Attribute
	{
	}
}

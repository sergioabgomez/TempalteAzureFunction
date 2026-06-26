namespace FunctionTemplate.Infrastructure.Attributes
{
	/// <summary>
	/// Marks a property to be bound from the HTTP request header with the given name.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class FromHeaderAttribute : Attribute
	{
		public string HeaderName { get; }

		public FromHeaderAttribute(string headerName)
		{
			HeaderName = headerName;
		}
	}
}

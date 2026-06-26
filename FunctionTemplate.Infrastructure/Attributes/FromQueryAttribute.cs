namespace FunctionTemplate.Infrastructure.Attributes
{
	/// <summary>
	/// Marks a property to be bound from the query string.
	/// Optionally specify an explicit query parameter name;
	/// otherwise the property name is converted to <c>snake_case</c>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class FromQueryAttribute : Attribute
	{
		public string? Name { get; }

		public FromQueryAttribute()
		{
		}

		public FromQueryAttribute(string name)
		{
			Name = name;
		}
	}
}

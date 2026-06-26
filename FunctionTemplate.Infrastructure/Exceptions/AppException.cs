namespace FunctionTemplate.Infrastructure.Exceptions
{
	/// <summary>
	/// Custom exception for application-layer errors. Thrown from commands/query handlers
	/// to signal a specific HTTP status code and message to the global error handler.
	/// </summary>
	public class AppException : Exception
	{
		public int StatusCode { get; }

		/// <summary>
		/// Human-readable title for the HTTP status code, following RFC 9110.
		/// </summary>
		public string Title { get; }

		/// <summary>
		/// URI reference that identifies the error type (RFC 9457).
		/// Auto-resolved from the status code via <see cref="GetTypeForStatus"/>,
		/// or overridden via the <c>type</c> constructor parameter.
		/// </summary>
		public string Type { get; }

		public AppException(int statusCode, string message)
			: this(statusCode, message, type: GetTypeForStatus(statusCode))
		{
		}

		public AppException(int statusCode, string message, Exception innerException)
			: this(statusCode, message, innerException, type: GetTypeForStatus(statusCode))
		{
		}

		public AppException(int statusCode, string message, string type)
			: base(message)
		{
			StatusCode = statusCode;
			Title = GetTitleForStatus(statusCode);
			Type = type;
		}

		public AppException(int statusCode, string message, Exception innerException, string type)
			: base(message, innerException)
		{
			StatusCode = statusCode;
			Title = GetTitleForStatus(statusCode);
			Type = type;
		}

		private static string GetTitleForStatus(int statusCode) => statusCode switch
		{
			400 => "Bad Request",
			401 => "Unauthorized",
			403 => "Forbidden",
			404 => "Not Found",
			409 => "Conflict",
			422 => "Unprocessable Entity",
			429 => "Too Many Requests",
			>= 500 and < 600 => "Server Error",
			_ => "Application Error"
		};

		private static string GetTypeForStatus(int statusCode) => statusCode switch
		{
			400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
			401 => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
			403 => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
			404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
			409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
			422 => "https://tools.ietf.org/html/rfc9110#section-15.5.21",
			429 => "https://tools.ietf.org/html/rfc9110#section-15.5.15",
			>= 500 and < 600 => "https://tools.ietf.org/html/rfc9110#section-15.6.1",
			_ => "about:blank"
		};
	}
}

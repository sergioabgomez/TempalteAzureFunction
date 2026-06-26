using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Reflection;
using FunctionTemplate.Infrastructure.Attributes;
using System.Collections.Specialized;

namespace FunctionTemplate.Infrastructure.Helpers
{
	/// <summary>
	/// Binds an <see cref="HttpRequestData"/> to a typed object. Each property
	/// resolves its source from an attribute:
	/// <list type="bullet">
	///   <item><see cref="FromHeaderAttribute"/> — from request headers.</item>
	///   <item><see cref="FromQueryAttribute"/> — from query string.</item>
	///   <item><see cref="FromBodyJsonAttribute"/> — from body JSON only.</item>
	///   <item>No attribute — body first, fallback to query string.</item>
	/// </list>
	/// Query parameter names are expected in <c>snake_case</c> and are automatically
	/// mapped to PascalCase C# properties.
	/// </summary>
	public static class BindObject
	{
		/// <summary>
		/// Creates a new <typeparamref name="T"/> and populates each property from
		/// its designated source (attribute-driven).
		/// </summary>
		/// <returns>A populated instance, or <c>null</c> if no property could be bound.</returns>
		public static async Task<T?> BindAsync<T>(HttpRequestData req, CancellationToken cancellationToken = default)
			where T : class, new()
		{
			string body;
			using (var reader = new StreamReader(req.Body, leaveOpen: false))
			{
				body = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
			}

			object? bodyObj = null;
			if (!string.IsNullOrWhiteSpace(body))
			{
				bodyObj = JsonConvert.DeserializeObject<T>(body, new JsonSerializerSettings
				{
					ContractResolver = new DefaultContractResolver
					{
						NamingStrategy = new SnakeCaseNamingStrategy()
					},
				});
			}

			var result = new T();
			PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			NameValueCollection query = req.Query;
			var anyBound = false;

			foreach (PropertyInfo prop in props)
			{
				if (!prop.CanWrite)
				{
					continue;
				}

				FromHeaderAttribute? headerAttr = prop.GetCustomAttribute<FromHeaderAttribute>();

				if (headerAttr is not null)
				{
					if (TryBindHeader(req, prop, headerAttr, result))
					{
						anyBound = true;
					}

					continue;
				}

				FromQueryAttribute? queryAttr = prop.GetCustomAttribute<FromQueryAttribute>();

				if (queryAttr is not null)
				{
					if (TryBindQuery(query, prop, queryAttr, result))
					{
						anyBound = true;
					}

					continue;
				}

				if (prop.GetCustomAttribute<FromBodyJsonAttribute>() is not null)
				{
					if (bodyObj is not null && TryBindFromBody(bodyObj, prop, result))
					{
						anyBound = true;
					}

					continue;
				}

				var bound = false;

				if (bodyObj is not null)
				{
					bound = TryBindFromBody(bodyObj, prop, result);
				}

				if (!bound)
				{
					bound = TryBindQuery(query, prop, null, result);
				}

				if (bound)
				{
					anyBound = true;
				}
			}

			return anyBound ? result : null;
		}

		private static bool TryBindFromBody(object bodyObj, PropertyInfo prop, object target)
		{
			Type bodyType = bodyObj.GetType();
			PropertyInfo? bodyProp = bodyType.GetProperty(prop.Name);

			if (bodyProp is null)
			{
				return false;
			}

			var value = bodyProp.GetValue(bodyObj);

			if (value is null)
			{
				return false;
			}

			prop.SetValue(target, value);

			return true;
		}

		private static bool TryBindQuery(NameValueCollection query, PropertyInfo prop, FromQueryAttribute? queryAttr, object target)
		{
			var key = queryAttr?.Name;
			var queryValue = key is not null ? query[key] : query[PascalToSnakeCase(prop.Name)] ?? query[prop.Name];

			if (string.IsNullOrEmpty(queryValue))
			{
				return false;
			}

			Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

			try
			{
				var typedValue = targetType switch
				{
					{ } typed when typed == typeof(string) => queryValue,
					{ } typed when typed == typeof(Guid) => Guid.Parse(queryValue),
					{ } typed when typed == typeof(long) => long.Parse(queryValue, CultureInfo.InvariantCulture),
					{ } typed when typed == typeof(int) => int.Parse(queryValue, CultureInfo.InvariantCulture),
					{ } typed when typed == typeof(bool) => ParseBool(queryValue),
					{ } typed when typed == typeof(DateTime) => DateTime.Parse(queryValue, CultureInfo.InvariantCulture),
					{ } typed when typed == typeof(decimal) => decimal.Parse(queryValue, CultureInfo.InvariantCulture),
					{ } typed when typed == typeof(double) => double.Parse(queryValue, CultureInfo.InvariantCulture),
					_ => Convert.ChangeType(queryValue, targetType, CultureInfo.InvariantCulture),
				};

				prop.SetValue(target, typedValue);

				return true;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(
					$"Failed to bind query parameter '{key ?? PascalToSnakeCase(prop.Name)}' " +
					$"to property '{prop.Name}' of type '{prop.PropertyType.Name}'. " +
					$"Value: '{queryValue}'. Error: {ex.Message}");
			}
		}

		private static bool TryBindHeader(HttpRequestData req, PropertyInfo prop, FromHeaderAttribute attr, object target)
		{
			if (!req.Headers.TryGetValues(attr.HeaderName, out IEnumerable<string>? values))
			{
				return false;
			}

			var headerValue = values.FirstOrDefault();
			if (string.IsNullOrEmpty(headerValue))
			{
				return false;
			}

			Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

			try
			{
				var typedValue = targetType switch
				{
					{ } typed when typed == typeof(string) => headerValue,
					_ => Convert.ChangeType(headerValue, targetType, CultureInfo.InvariantCulture),
				};

				prop.SetValue(target, typedValue);

				return true;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(
					$"Failed to bind header '{attr.HeaderName}' to property '{prop.Name}' " +
					$"of type '{prop.PropertyType.Name}'. Value: '{headerValue}'. Error: {ex.Message}");
			}
		}

		/// <summary>
		/// Same as <see cref="BindAsync{T}"/> but throws if the result is <c>null</c>.
		/// </summary>
		public static async Task<T> BindRequiredAsync<T>(HttpRequestData req, CancellationToken cancellationToken = default)
			where T : class, new()
		{
			T? result = await BindAsync<T>(req, cancellationToken).ConfigureAwait(false);
			return result ?? throw new InvalidOperationException(
				$"No request body or query parameters found to bind to {typeof(T).Name}.");
		}

		private static bool ParseBool(string value)
		{
			return value switch
			{
				"1" or "true" or "yes" or "on" => true,
				"0" or "false" or "no" or "off" => false,
				_ => bool.Parse(value),
			};
		}

		private static string PascalToSnakeCase(string pascal)
		{
			if (string.IsNullOrEmpty(pascal))
			{
				return pascal;
			}

			return string.Concat(
				pascal.Select((c, i) =>
					i > 0 && char.IsUpper(c)
						? "_" + char.ToLowerInvariant(c)
						: char.ToLowerInvariant(c).ToString()));
		}
	}
}

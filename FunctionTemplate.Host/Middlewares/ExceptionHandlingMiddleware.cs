using Cortex.Mediator.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Newtonsoft.Json;
using FunctionTemplate.Infrastructure.Exceptions;
using System.Net;

namespace FunctionTemplate.Host.Middlewares
{
	internal sealed class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
	{
		private static readonly JsonSerializerSettings JsonSettings = new()
		{
			ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
		};

		public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (AppException ex)
			{
				HttpContext? httpContext = context.GetHttpContext();

				if (httpContext?.Response.HasStarted == false)
				{
					var problem = new ProblemDetails
					{
						Type = ex.Type,
						Title = ex.Title,
						Status = ex.StatusCode,
						Detail = ex.Message,
						Instance = httpContext.Request.Path
					};

					httpContext.Response.StatusCode = ex.StatusCode;
					httpContext.Response.ContentType = "application/problem+json";
					await httpContext.Response.WriteAsync(
						JsonConvert.SerializeObject(problem, JsonSettings));
				}
			}
			catch (ValidationException ex)
			{
				HttpContext? httpContext = context.GetHttpContext();

				if (httpContext?.Response.HasStarted == false)
				{
					var problem = new ProblemDetails
					{
						Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
						Title = "Validation Error",
						Status = (int)HttpStatusCode.BadRequest,
						Detail = "One or more validation errors occurred.",
						Instance = httpContext.Request.Path
					};

					problem.Extensions["errors"] = ex.Errors;

					httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					httpContext.Response.ContentType = "application/problem+json";
					await httpContext.Response.WriteAsync(
						JsonConvert.SerializeObject(problem, JsonSettings));
				}
			}
			catch (Exception ex)
			{
				HttpContext? httpContext = context.GetHttpContext();

				if (httpContext?.Response.HasStarted == false)
				{
					var problem = new ProblemDetails
					{
						Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
						Title = "Internal Server Error",
						Status = (int)HttpStatusCode.InternalServerError,
						Detail = "An unexpected error occurred.",
						Instance = httpContext.Request.Path
					};

					problem.Extensions["trace"] = ex.ToString();

					httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					httpContext.Response.ContentType = "application/problem+json";
					await httpContext.Response.WriteAsync(
						JsonConvert.SerializeObject(problem, JsonSettings));
				}
			}
		}
	}
}

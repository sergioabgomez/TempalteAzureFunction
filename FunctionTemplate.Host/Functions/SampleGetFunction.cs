using System.Net;
using Cortex.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using FunctionTemplate.Application.Handlers.Queries.Sample;
using FunctionTemplate.Application.Models.Responses;
using FunctionTemplate.Infrastructure.Helpers;

namespace FunctionTemplate.Host.Functions;

public class SampleGetFunction
{
	private readonly IMediator mediator;
	private readonly ILogger<SampleGetFunction> logger;

	public SampleGetFunction(IMediator mediator, ILogger<SampleGetFunction> logger)
	{
		this.mediator = mediator;
		this.logger = logger;
	}

	[Function("SampleGet")]
	[OpenApiOperation(operationId: "SampleGet", tags: ["Sample"])]
	[OpenApiParameter("name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Name to greet")]
	[OpenApiParameter("uppercase", In = ParameterLocation.Query, Required = false, Type = typeof(bool), Description = "Whether to return in uppercase")]
	[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SampleResponse))]
	public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext executionContext)
	{
		logger.LogInformation("Processing SampleGet request");

		SampleQuery query = await BindObject.BindRequiredAsync<SampleQuery>(req);

		SampleResponse result = await mediator.SendQueryAsync<SampleQuery, SampleResponse>(query);

		HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
		await response.WriteAsJsonAsync(result);

		return response;
	}
}

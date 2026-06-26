using System.Net;
using Cortex.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using FunctionTemplate.Application.Handlers.Commands.CreateSample;
using FunctionTemplate.Application.Models.Requests;
using FunctionTemplate.Application.Models.Responses;
using FunctionTemplate.Infrastructure.Helpers;

namespace FunctionTemplate.Host.Functions;

public class SamplePostFunction
{
	private readonly IMediator mediator;
	private readonly ILogger<SamplePostFunction> logger;

	public SamplePostFunction(IMediator mediator, ILogger<SamplePostFunction> logger)
	{
		this.mediator = mediator;
		this.logger = logger;
	}

	[Function("SamplePost")]
	[OpenApiOperation(operationId: "SamplePost", tags: ["Sample"])]
	[OpenApiRequestBody("application/json", typeof(CreateSampleRequest))]
	[OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(CreateSampleResponse))]
	public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext executionContext)
	{
		logger.LogInformation("Processing SamplePost request");

		CreateSampleCommand command = await BindObject.BindRequiredAsync<CreateSampleCommand>(req);

		CreateSampleResponse result = await mediator.SendCommandAsync<CreateSampleCommand, CreateSampleResponse>(command);

		HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
		await response.WriteAsJsonAsync(result);

		return response;
	}
}

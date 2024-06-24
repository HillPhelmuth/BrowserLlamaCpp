using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api.Functions;

public class LlamaCppModels(ILoggerFactory loggerFactory, HuggingFaceService huggingFaceService)
{
	private readonly ILogger _logger = loggerFactory.CreateLogger<LlamaCppModels>();

	[Function("LlamaCppModels")]
	public async ValueTask<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "LlamaCppModels/{search}/{filter}/{count}")] HttpRequestData req, string search, string filter, int count)
	{
		_logger.LogInformation("C# HTTP trigger function processed a request.");
		_logger.LogInformation("Uri in HttpRequestData: {uri}", req.Url.ToString());
		var response = req.CreateResponse(HttpStatusCode.OK);
		//response.Headers.Add("Content-Type", "application/json; charset=utf-8");
		var results = await huggingFaceService.GetModelsAsync(search, filter, count);
		//response.WriteString("Welcome to Azure Functions!");
		await response.WriteAsJsonAsync(results);
		return response;
	}
        
}
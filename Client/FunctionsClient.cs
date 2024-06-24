using System.Net.Http.Json;
using BrowserLlamaCpp.Shared;

namespace BrowserLlamaCpp.Client;

public class FunctionsClient
{
	private readonly HttpClient _client;
	private readonly IConfiguration _configuration;

	public FunctionsClient(HttpClient client, IConfiguration configuration)
	{
		_client = client;
		_configuration = configuration;
	}
	public async Task<List<HuggingFaceModel>> GetGGufModels(string search, string filterType, int count)
	{
		try
		{
			var results = await _client.GetFromJsonAsync<List<HuggingFaceModel>>($"/api/LlamaCppModels/{search}/{filterType}/{count}");
			return results ?? [];
			
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return [];
		}
	}
}
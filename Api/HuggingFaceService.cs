using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using BrowserLlamaCpp.Shared;

namespace Api;


public class HuggingFaceService(IConfiguration configuration)
{
	public async Task<List<HuggingFaceModel>> GetModelsAsync(string search, string filter, int limit = 20, bool full = true, HttpClient? client = null)
	{
		client ??= new HttpClient();
		var isFull = full ? bool.TrueString : bool.FalseString;
		var url = $"https://huggingface.co/api/models?search={search}&filter={filter}&limit={limit}&full={isFull}";

		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["HuggingFace:ApiKey"]);
		//var models = await client.GetFromJsonAsync<List<HuggingFaceModel>>(url);
		var response = await client.GetAsync(url);
		//return models ?? [];
		response.EnsureSuccessStatusCode();

		var responseBody = await response.Content.ReadAsStringAsync();

		return JsonSerializer.Deserialize<List<HuggingFaceModel>>(responseBody)!;
	}
}
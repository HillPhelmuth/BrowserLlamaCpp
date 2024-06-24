using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BrowserLlamaCpp.Shared;

public class HuggingFaceService(HttpClient client, IConfiguration configuration)
{
	public async Task<List<HuggingFaceModel>> GetRequestAsync(string search, string filter, int limit = 20, bool full = true)
	{
		var isFull = full ? bool.TrueString : bool.FalseString;
		var url = $"https://huggingface.co/api/models?search={search}&filter={filter}&limit={limit}&full={isFull}";

		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["HuggingFace:ApiKey"]);
		//var models = await client.GetFromJsonAsync<List<HuggingFaceModel>>(url);
		var response = await client.GetAsync(url);
		//return models ?? [];
		response.EnsureSuccessStatusCode();

		var responseBody = await response.Content.ReadAsStringAsync();

		return JsonSerializer.Deserialize<List<HuggingFaceModel>>(responseBody);
	}
}
public class HfRequestMdoel
{
	public string? SearchTerm { get; set; }
	public int Count { get; set; }
	public string FilterType { get; set; }
}
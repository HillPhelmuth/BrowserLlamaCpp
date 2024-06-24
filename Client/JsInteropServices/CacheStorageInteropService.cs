using Microsoft.JSInterop;

namespace BrowserLlamaCpp.Client.JsInteropServices;

public class CacheStorageInteropService(IJSRuntime jsRuntime)
	: JsModule(jsRuntime, "./js/cacheStorageAccessor.js")
{
	public async Task StoreAsync(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage)
	{
		
		string requestMethod = requestMessage.Method.Method;
		string requestBody = await GetRequestBodyAsync(requestMessage);
		string responseBody = await responseMessage.Content.ReadAsStringAsync();

		await InvokeVoidAsync("store", requestMessage.RequestUri, requestMethod, requestBody, responseBody);
	}

	public async Task<string> GetAsync(HttpRequestMessage requestMessage)
	{
		
		string requestMethod = requestMessage.Method.Method;
		string requestBody = await GetRequestBodyAsync(requestMessage);
		string result = await InvokeAsync<string>("get", requestMessage.RequestUri, requestMethod, requestBody);

		return result;
	}
	public async ValueTask<IEnumerable<string>> GetModelsAsync()
	{
		return await InvokeAsync<string[]>("getAllModels");
	}

	public async Task RemoveAsync(HttpRequestMessage requestMessage)
	{
		
		string requestMethod = requestMessage.Method.Method;
		string requestBody = await GetRequestBodyAsync(requestMessage);
		await InvokeVoidAsync("remove", requestMessage.RequestUri, requestMethod, requestBody);
	}

	public async Task RemoveAllAsync()
	{
		
		await InvokeVoidAsync("removeAll");
	}

	private static async Task<string> GetRequestBodyAsync(HttpRequestMessage requestMessage)
	{
		string requestBody = "";

		if (requestMessage.Content is not null)
		{
			requestBody = await requestMessage.Content.ReadAsStringAsync() ?? "";
		}

		return requestBody;
	}
}
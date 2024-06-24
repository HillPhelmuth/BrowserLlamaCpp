using System.Reflection;
using System.Text.Json;
using Microsoft.JSInterop;

namespace BrowserLlamaCpp.Client.JsInteropServices;

public abstract class JsInteropService(IJSRuntime jSRuntime, string filePath) : JsModule(jSRuntime, filePath)
{
	protected abstract DotNetObjectReference<JsInteropService> DotNetObjectRef { get; }
	public event Action<string>? UpdateResult;
	public event Action? ModelLoaded;
	public event Action? ResultComplete;
	public abstract ValueTask Init();
	public ValueTask LoadAndRun(string url, string prompt)
	{
		return InvokeVoidAsync("loadAndExecuteModel", url, prompt);
	}

	[JSInvokable("UpdateResult")]
	public void OnUpdateResult(string message) => UpdateResult?.Invoke(message);

	[JSInvokable("ModelLoaded")]
	public void OnModelLoaded() => ModelLoaded?.Invoke();

	[JSInvokable("UpdateComplete")]
	public void OnResultComplete() => ResultComplete?.Invoke();
}

public abstract class JsModule(IJSRuntime js, string moduleUrl) : IAsyncDisposable
{
	private readonly Task<IJSObjectReference> moduleTask = js.InvokeAsync<IJSObjectReference>("import", moduleUrl).AsTask();
	private bool disposedValue;

	protected async ValueTask InvokeVoidAsync(string identifier, params object[]? args)
		=> await (await moduleTask).InvokeVoidAsync(identifier, args);
	protected async ValueTask<T> InvokeAsync<T>(string identifier, params object[]? args)
		=> await (await moduleTask).InvokeAsync<T>(identifier, args);
	// Release the JS module
	protected virtual async ValueTask DisposeAsync(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				await (await moduleTask).DisposeAsync();
			}

			disposedValue = true;
		}
	}

	public async ValueTask DisposeAsync()
	{
		// Do not change this code. Put cleanup code in 'DisposeAsync(bool disposing)' method
		await DisposeAsync(disposing: true);
		GC.SuppressFinalize(this);

	}
}

public static class Exts
{
	public static T ExtractFromAssembly<T>(string fileName)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var jsonName = assembly.GetManifestResourceNames()
			.SingleOrDefault(s => s.EndsWith(fileName, StringComparison.OrdinalIgnoreCase)) ?? "";
		using var stream = assembly.GetManifestResourceStream(jsonName);
		using var reader = new StreamReader(stream);
		object result = reader.ReadToEnd();
		if (typeof(T) == typeof(string))
			return (T)result;
		return JsonSerializer.Deserialize<T>(result.ToString());
	}
	public static IServiceCollection AddJsInteropServices(this IServiceCollection services)
	{
		services.AddSingleton<SingleThreadInteropService>();
		services.AddSingleton<MultiThreadInteropService>();
		services.AddSingleton<GgufInteropService>();
		services.AddSingleton<CacheStorageInteropService>();
		return services;
	}
}
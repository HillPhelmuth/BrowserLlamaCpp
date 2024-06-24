using Microsoft.JSInterop;

namespace BrowserLlamaCpp.Client.JsInteropServices;

public sealed class GgufInteropService(IJSRuntime jsRuntime)
	: JsModule(jsRuntime, "./js/gguf.js")
{
	private IJSRuntime _jsRuntime = jsRuntime;
	public ValueTask<object> GetGgufModelSpecs(string modelUrl)
	{
		return _jsRuntime.InvokeAsync<object>("Gguf.GetModelSpecs", modelUrl);
	}
}
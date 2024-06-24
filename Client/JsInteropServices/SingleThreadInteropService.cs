using Microsoft.JSInterop;

namespace BrowserLlamaCpp.Client.JsInteropServices;

public sealed class SingleThreadInteropService(IJSRuntime jsRuntime)
	: JsInteropService(jsRuntime, "./js/example-single-thread.js")
{
	protected override DotNetObjectReference<JsInteropService> DotNetObjectRef => DotNetObjectReference.Create<JsInteropService>(this);
	public override ValueTask Init() => InvokeVoidAsync("init", DotNetObjectRef);
}
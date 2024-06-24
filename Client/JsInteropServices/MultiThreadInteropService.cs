using Microsoft.JSInterop;

namespace BrowserLlamaCpp.Client.JsInteropServices;

public sealed class MultiThreadInteropService(IJSRuntime jsRuntime)
	: JsInteropService(jsRuntime, "./js/example-multi-thread.js")
{
	
	protected override DotNetObjectReference<JsInteropService> DotNetObjectRef => DotNetObjectReference.Create<JsInteropService>(this);
	public override ValueTask Init() => InvokeVoidAsync("init", DotNetObjectRef);
}
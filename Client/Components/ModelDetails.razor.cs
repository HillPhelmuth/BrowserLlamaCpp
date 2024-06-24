using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BrowserLlamaCpp.Client.JsInteropServices;
using BrowserLlamaCpp.Shared;
using Radzen;

namespace BrowserLlamaCpp.Client.Components;

public partial class ModelDetails : ComponentBase
{
	[Inject]
	private DialogService DialogService { get; set; } = default!;
	[Inject]
	private GgufInteropService GgufInteropService { get; set; } = default!;
	[Parameter]
	public string ModelUrl { get; set; } = string.Empty;
	[Parameter]
	public string ModelName { get; set; } = string.Empty;
	[Parameter]
	public double ModelSize { get; set; }
	private string _output = string.Empty;
	private Dictionary<string, object> _outputProperties = [];
	protected override async Task OnParametersSetAsync()
	{
		var details = await GgufInteropService.GetGgufModelSpecs(ModelUrl);
		var json = JsonSerializer.Serialize(details);
		_outputProperties = JsonSerializer.Deserialize<Dictionary<string, object>>(json)?.Where(x => IsSpecificType(x.Value)).ToDictionary() ?? [];
		_output = JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true });
		StateHasChanged();
		await base.OnParametersSetAsync();
	}
	protected override Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{

		}
		return base.OnAfterRenderAsync(firstRender);
	}
	private void Cancel()
	{
		DialogService.Close(false);
	}
	private void Select()
	{
		DialogService.Close(true);
	}
	private static bool IsSpecificType(object obj)
	{
		if (obj == null) return false;

		Type type = obj.GetType();
		if (obj is JsonElement element)
		{
			JsonValueKind kind = element.ValueKind;
			return kind is JsonValueKind.String or JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False;
		}
		//Console.WriteLine(type.Name);
		return false;
	}
}
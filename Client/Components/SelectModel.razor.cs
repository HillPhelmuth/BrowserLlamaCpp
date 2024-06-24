using BrowserLlamaCpp.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BrowserLlamaCpp.Client.Components;

public partial class SelectModel : ComponentBase
{
	[Inject]
	private FunctionsClient FunctionsClient { get; set; } = default!;
	[Inject]
	private DialogService DialogService { get; set; } = default!; 
	[Parameter]
	public EventCallback<List<HuggingFaceModel>> ModelsRetreived { get; set; }
	[Parameter] 
	public string ModelUrl { get; set; } = string.Empty;
	[Parameter]
	public EventCallback<string> ModelUrlChanged { get; set; }
	private List<HuggingFaceModel> _huggingFaceModels = [];
	private class SearchForm
	{
		public string SearchTerm { get; set; } = string.Empty;
		public string FilterType { get; set; } = "gguf";
		public int Count { get; set; } = 10;
	}
	private SearchForm _searchForm = new();
	private async void Submit(SearchForm search)
	{
		var results = await FunctionsClient.GetGGufModels(search.SearchTerm, search.FilterType, search.Count);
		_huggingFaceModels = results;
		await ModelsRetreived.InvokeAsync(results);
	}
	private async Task ShowModelDetails(string url, string name)
	{
		var options = new DialogOptions { Style = "width: max-content; height:40rem", ShowClose = false, ShowTitle = true, Resizable = true, CloseDialogOnOverlayClick = false };
		var fileSize = await Helpers.GetFileSizeFromUrl(url);
		var parameters = new Dictionary<string, object> { ["ModelUrl"] = url, ["ModelName"] = name, ["ModelSize"] = fileSize };
		var result = await DialogService.OpenAsync<ModelDetails>($"Model Details", parameters, options);
		if (result == true)
		{
			ModelUrl = url;
			await ModelUrlChanged.InvokeAsync(url);
		}
	}
}
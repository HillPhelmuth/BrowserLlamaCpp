using BrowserLlamaCpp.Client.JsInteropServices;
using BrowserLlamaCpp.Shared;
using Microsoft.AspNetCore.Components;

namespace BrowserLlamaCpp.Client.Pages;

public partial class LoadAndRun : ComponentBase
{
    [Inject]
    private SingleThreadInteropService SingleThreadInteropService { get; set; } = default!;
    [Inject]
    private MultiThreadInteropService MultiThreadInteropService { get; set; } = default!;
    [Inject]
    private CacheStorageInteropService CacheStorageInteropService { get; set; } = default!;
    private ModelState _modelState = ModelState.InActive;
    private string _output = string.Empty;
    private string _busyText => _modelState switch
    {
        ModelState.Loading => "Loading...",
        ModelState.Running => "Running...",
        _ => string.Empty
    };
    private class InputForm
    {
        public string Search { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Prompt { get; set; } = "Tell me something interesting in 100 words or less.";
        public ThreadOption ThreadOption { get; set; }
    }
        
    private InputForm _inputForm = new();
    private List<TestedModels> _models = Enum.GetValues<TestedModels>().ToList();
    private Dictionary<string, string> ModelNameUrls { get; set; } = [];
    private List<HuggingFaceModel> _huggingFaceModels = [];
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SingleThreadInteropService.Init();
            SingleThreadInteropService.UpdateResult += HandleUpdateResult;
            SingleThreadInteropService.ModelLoaded += HandleModelLoaded;
            SingleThreadInteropService.ResultComplete += HandleResultComplete;
            await MultiThreadInteropService.Init();
            MultiThreadInteropService.UpdateResult += HandleUpdateResult;
            MultiThreadInteropService.ModelLoaded += HandleModelLoaded;
            MultiThreadInteropService.ResultComplete += HandleResultComplete;
            var loaded = (await CacheStorageInteropService.GetModelsAsync()).ToList();
            Console.WriteLine($"Loaded {loaded.Count} models from cache");
            foreach (var model in loaded)
            {
                var substringAfterLastSlash = GetSubstringAfterLastSlash(model);
                Console.WriteLine($"{substringAfterLastSlash} found in cache. (url = '{model}')");
                var success = ModelNameUrls.TryAdd(substringAfterLastSlash, model);
            }
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }
    private void HandleModels(List<HuggingFaceModel> models)
    {
        _huggingFaceModels = models;
        var allModelNames = _huggingFaceModels.SelectMany(x => x.DownloadUrls());
        foreach (var item in allModelNames)
        {
            ModelNameUrls.Add(item.Key, item.Value);
        }
        StateHasChanged();
    }
    private void HandleResultComplete()
    {
        _modelState = ModelState.InActive;
        StateHasChanged();
    }

    private void HandleModelLoaded()
    {
        _modelState = ModelState.Running;
        StateHasChanged();
    }

    private void HandleUpdateResult(string message)
    {
        _output += message;
        StateHasChanged();
    }

    private async void Submit(InputForm input)
    {
        if (input.ThreadOption == ThreadOption.SingleThread)
            await SingleThreadInteropService.LoadAndRun(input.Url, input.Prompt);
        else
            await MultiThreadInteropService.LoadAndRun(input.Url, input.Prompt);
        _modelState = ModelState.Loading;
        StateHasChanged();
    }
    private static string GetSubstringAfterLastSlash(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        var lastSlashIndex = input.LastIndexOf('/');
        if (lastSlashIndex == -1 || lastSlashIndex == input.Length - 1) return string.Empty;
        return input[(lastSlashIndex + 1)..];
    }
}
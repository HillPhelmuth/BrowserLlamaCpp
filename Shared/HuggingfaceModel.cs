using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BrowserLlamaCpp.Shared;

public class HuggingFaceModel
{
	[JsonPropertyName("_id")]
	public string Id { get; set; }

	[JsonPropertyName("id")]
	public string HuggingFaceModelId { get; set; }

	[JsonPropertyName("author")]
	public string Author { get; set; }


	[JsonPropertyName("lastModified")]
	public string LastModified { get; set; }

	[JsonPropertyName("likes")]
	public long Likes { get; set; }

	[JsonPropertyName("private")]
	public bool Private { get; set; }

	[JsonPropertyName("sha")]
	public string Sha { get; set; }

	[JsonPropertyName("downloads")]
	public long Downloads { get; set; }

	[JsonPropertyName("tags")]
	public List<string> Tags { get; set; }

	[JsonPropertyName("pipeline_tag")]
	public string PipelineTag { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	[JsonPropertyName("library_name")]
	public string LibraryName { get; set; }

	[JsonPropertyName("createdAt")]
	public string CreatedAt { get; set; }

	[JsonPropertyName("modelId")]
	public string ModelId { get; set; }

	[JsonPropertyName("siblings")]
	public List<Sibling> Siblings { get; set; }
	private const string HfBaseUrl = "https://huggingface.co";
	public Dictionary<string,string> DownloadUrls()
	{
		return Siblings.Where(sibling => sibling.Rfilename.EndsWith(".gguf", System.StringComparison.OrdinalIgnoreCase))
			.ToDictionary(x => $"{Author}-{HuggingFaceModelId}-{x.Rfilename}" ,sibling => $"{HfBaseUrl}/{HuggingFaceModelId}/resolve/main/{sibling.Rfilename}");
	}
}

public partial class Sibling
	{
		[JsonPropertyName("rfilename")]
		public string Rfilename { get; set; }
	}
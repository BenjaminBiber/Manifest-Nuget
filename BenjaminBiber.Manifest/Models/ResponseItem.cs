using System.Text.Json.Serialization;

namespace BenjaminBiber.Manifest.Models;

public class ResponseItem<T>
{
    public ResponseItem(List<T> data)
    {
        Data = data;
    }

    public ResponseItem()
    {
        Data = [];
    }

    [JsonPropertyName("data")]
    public List<T> Data { get; set; }

    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("lastPage")]
    public int LastPage { get; set; }

    [JsonPropertyName("from")]
    public int From { get; set; }

    [JsonPropertyName("to")]
    public int To { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("perPage")]
    public int PerPage { get; set; }
}
using System.Text.Json.Serialization;

namespace BenjaminBiber.Manifest.Models;

public class ResponseItem<T>
{
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="ResponseItem{T}"/>-Klasse mit den angegebenen Daten.
    /// </summary>
    /// <param name="data">Die Liste der Daten, die in der Antwort enthalten sind.</param>
    public ResponseItem(List<T> data)
    {
        Data = data;
    }
    
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="ResponseItem{T}"/>-Klasse mit einer leeren Datenliste.
    /// </summary>
    public ResponseItem()
    {
        Data = [];
    }
    
    /// <summary>
    /// Gibt die Liste der Daten zurück, die in der Antwort enthalten sind, oder legt diese fest.
    /// </summary>
    [JsonPropertyName("data")]
    public List<T> Data { get; set; }
    
    /// <summary>
    /// Gibt die aktuelle Seite der Antwort an oder legt diese fest.
    /// </summary>
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }
    
    /// <summary>
    /// Gibt die letzte Seite der Antwort an oder legt diese fest.
    /// </summary>
    [JsonPropertyName("lastPage")]
    public int LastPage { get; set; }
    
    /// <summary>
    /// Gibt den Startindex der Daten auf der aktuellen Seite an oder legt diesen fest.
    /// </summary>
    [JsonPropertyName("from")]
    public int From { get; set; }
    
    /// <summary>
    /// Gibt den Endindex der Daten auf der aktuellen Seite an oder legt diesen fest.
    /// </summary>
    [JsonPropertyName("to")]
    public int To { get; set; }
    
    /// <summary>
    /// Gibt die Gesamtanzahl der verfügbaren Daten an oder legt diese fest.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }
    
    /// <summary>
    /// Gibt die Anzahl der Elemente pro Seite an oder legt diese fest.
    /// </summary>
    [JsonPropertyName("perPage")]
    public int PerPage { get; set; }
}
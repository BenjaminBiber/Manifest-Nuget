namespace BenjaminBiber.Manifest.Models;

public sealed class CacheItem
{
    /// <summary>
    /// Gibt das Ablaufdatum und die Uhrzeit des Cache-Eintrags an.
    /// </summary>
    public DateTimeOffset ExpiresAt { get; init; }
    
    /// <summary>
    /// Enthält einen Lazy-Wert, der eine asynchrone Operation repräsentiert und den zwischengespeicherten Wert liefert.
    /// </summary>
    public Lazy<Task<object?>> LazyValue { get; init; } = default!;
}
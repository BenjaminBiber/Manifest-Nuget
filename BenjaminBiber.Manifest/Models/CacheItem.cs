namespace BenjaminBiber.Manifest.Models;

public sealed class CacheItem
{
    public DateTimeOffset ExpiresAt { get; init; }
    public Lazy<Task<object?>> LazyValue { get; init; } = default!;
}
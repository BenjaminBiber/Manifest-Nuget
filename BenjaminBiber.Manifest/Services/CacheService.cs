using System.Collections.Concurrent;
using BenjaminBiber.Manifest.Interfaces;
using BenjaminBiber.Manifest.Models;
namespace BenjaminBiber.Manifest.Services;
using System.Collections.Concurrent;

public sealed class CacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CacheItem> _cache = new();
    private readonly TimeSpan _defaultTtl;
    
    public CacheService(TimeSpan? defaultTtl = null)
    {
        _defaultTtl = defaultTtl ?? TimeSpan.FromHours(1);
    }
    
    /// <summary>
    /// Retrieves an item from the cache if it exists and is not expired. 
    /// If the item does not exist or is expired, it uses the provided factory function to create the item, 
    /// stores it in the cache, and returns the newly created item.
    /// </summary>
    /// <typeparam name="T">The type of the item to retrieve or add to the cache.</typeparam>
    /// <param name="key">The unique key identifying the cached item.</param>
    /// <param name="factory">A function that generates the item to cache if it does not already exist.</param>
    /// <param name="ttl">An optional time-to-live (TTL) for the cached item. If not provided, the default TTL is used.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the factory function to complete.</param>
    /// <returns>The cached item, either retrieved from the cache or created by the factory function.</returns>
    public async Task<T> GetOrAddAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? ttl = null,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        // Check if the item exists in the cache and is not expired
        if (_cache.TryGetValue(key, out var existing) && existing.ExpiresAt > now)
        {
            var obj = await existing.LazyValue.Value;
            return (T?)obj!;
        }

        // Create a new cache item with the specified or default TTL
        var CacheItem = new CacheItem
        {
            ExpiresAt = now.Add(ttl ?? _defaultTtl),
            LazyValue = new Lazy<Task<object?>>(async () => await factory(ct))
        };

        // Add or update the cache with the new item
        var winner = _cache.AddOrUpdate(
            key,
            CacheItem,
            (_, old) => old.ExpiresAt > now ? old : CacheItem);

        // Retrieve the value of the winning cache item
        var valueObj = await winner.LazyValue.Value;

        // Remove the item from the cache if it is already expired
        if (winner.ExpiresAt <= DateTimeOffset.UtcNow)
            _cache.TryRemove(key, out _);

        return (T?)valueObj!;
    }

    /// <summary>
    /// Retrieves an item from the cache if it exists; otherwise, uses the provided factory function
    /// to create the item, stores it in the cache, and returns the newly created item.
    /// </summary>
    /// <typeparam name="T">The type of the item to retrieve or add to the cache.</typeparam>
    /// <param name="key">The unique key identifying the cached item.</param>
    /// <param name="dataFactory">A function that generates the item to cache if it does not already exist.</param>
    /// <returns>The cached item, either retrieved from the cache or created by the factory function.</returns>
    public Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> dataFactory)
        => GetOrAddAsync(key, _ => dataFactory(), ttl: null, ct: default);

    /// <summary>
    /// Removes an item from the cache if it exists.
    /// </summary>
    /// <param name="key">The unique key identifying the cached item to remove.</param>
    public void Remove(string key) => _cache.TryRemove(key, out _);
    
    /// <summary>
    /// Clears all items from the cache.
    /// </summary>
    public void Clear() => _cache.Clear();
}

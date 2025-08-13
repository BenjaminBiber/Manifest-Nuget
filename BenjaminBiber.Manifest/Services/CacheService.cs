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
    /// Get Items from Cache, if they dont exist, then the datafactory adds them 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <param name="ttl"></param>
    /// <param name="ct"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> GetOrAddAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? ttl = null,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        if (_cache.TryGetValue(key, out var existing) && existing.ExpiresAt > now)
        {
            var obj = await existing.LazyValue.Value;
            return (T?)obj!;
        }

        var CacheItem = new CacheItem
        {
            ExpiresAt = now.Add(ttl ?? _defaultTtl),
            LazyValue = new Lazy<Task<object?>>(async () => await factory(ct))
        };

        var winner = _cache.AddOrUpdate(
            key,
            CacheItem,
            (_, old) => old.ExpiresAt > now ? old : CacheItem);

        var valueObj = await winner.LazyValue.Value;

        if (winner.ExpiresAt <= DateTimeOffset.UtcNow)
            _cache.TryRemove(key, out _);

        return (T?)valueObj!;
    }

    public Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> dataFactory)
        => GetOrAddAsync(key, _ => dataFactory(), ttl: null, ct: default);

    public void Remove(string key) => _cache.TryRemove(key, out _);

    public void Clear() => _cache.Clear();
}

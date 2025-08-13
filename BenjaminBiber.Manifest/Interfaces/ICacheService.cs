namespace BenjaminBiber.Manifest.Interfaces;

public interface ICacheService
{
    Task<T> GetOrAddAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? ttl = null,
        CancellationToken ct = default);

    void Remove(string key);
    void Clear();
}

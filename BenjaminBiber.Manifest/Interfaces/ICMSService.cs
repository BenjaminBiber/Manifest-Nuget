using BenjaminBiber.Manifest.Models;

namespace BenjaminBiber.Manifest.Interfaces;

public interface ICmsService
{
    public Task<ResponseItem<T>> GetItems<T>(string key, string relativeUrl, CancellationToken ct = default);
}
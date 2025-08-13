using System.Text.Json;
using BenjaminBiber.Manifest.Interfaces;
using BenjaminBiber.Manifest.Models;
namespace BenjaminBiber.Manifest.Services;

public class CmsService : ICmsService
{
    private readonly ICacheService _cacheService;
    private readonly HttpClient _httpClient;

    public CmsService(ICacheService cacheService, HttpClient httpClient)
    {
        _cacheService = cacheService;
        _httpClient = httpClient;
    }
    
    /// <summary>
    /// Retrieves items from the cache if they exist; otherwise, fetches the data from the specified relative URL,
    /// stores it in the cache, and returns the newly fetched items.
    /// </summary>
    /// <typeparam name="T">The type of the items to retrieve or fetch.</typeparam>
    /// <param name="key">The unique key identifying the cached items.</param>
    /// <param name="relativeUrl">The relative URL to fetch the data from if it is not in the cache.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved or fetched items.</returns>
    public async Task<ResponseItem<T>> GetItems<T>(string key, string relativeUrl, CancellationToken ct = default)
    {
        return await _cacheService.GetOrAddAsync<ResponseItem<T>>(
            key,
            async token =>
            {
                var result = await GetData<ResponseItem<T>>(relativeUrl, token);
                return result.Success && result.Data is not null
                    ? result.Data
                    : new ResponseItem<T>();
            },
            ct: ct);
    }

    /// <summary>
    /// Fetches data from the specified relative URL and deserializes it into the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the fetched data will be deserialized.</typeparam>
    /// <param name="relativeUrl">The relative URL to fetch the data from.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="ApiResult{T}"/> 
    /// indicating whether the operation was successful and the fetched data or an error message.
    /// </returns>
    private async Task<ApiResult<T>> GetData<T>(string relativeUrl, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(relativeUrl))
            return new ApiResult<T> { Success = false, Error = "relativeUrl is null or empty" };
    
        try
        {
            var resp = await _httpClient.GetAsync(relativeUrl, ct);
            resp.EnsureSuccessStatusCode();
            
            var json = await resp.Content.ReadAsStringAsync(ct);
            System.Console.WriteLine(json);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<T>(json, options);
    
            return new ApiResult<T> { Success = true, Data = data! };
        }
        catch (Exception ex)
        {
            return new ApiResult<T> { Success = false, Error = ex.Message };
        }
    }
}
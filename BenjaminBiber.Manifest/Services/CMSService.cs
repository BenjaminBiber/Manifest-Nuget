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
    /// Gets a Collection of Items from Manifest
    /// </summary>
    /// <param name="key"></param>
    /// <param name="relativeUrl"></param>
    /// <param name="ct"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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
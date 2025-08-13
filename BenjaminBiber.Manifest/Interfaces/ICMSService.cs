using BenjaminBiber.Manifest.Models;

namespace BenjaminBiber.Manifest.Interfaces;

public interface ICmsService
{
    /// <summary>
    /// Ruft eine Liste von Elementen aus dem CMS ab.
    /// </summary>
    /// <typeparam name="T">Der Typ der Elemente, die abgerufen werden sollen.</typeparam>
    /// <param name="key">Der eindeutige Schlüssel, der die Elemente identifiziert.</param>
    /// <param name="relativeUrl">Die relative URL, von der die Elemente abgerufen werden.</param>
    /// <param name="ct">Ein Abbruch-Token, das überwacht wird, während die Operation ausgeführt wird.</param>
    /// <returns>
    /// Ein Task, der ein <see cref="ResponseItem{T}"/> enthält, das die abgerufenen Elemente oder Fehlerinformationen bereitstellt.
    /// </returns>
    public Task<ResponseItem<T>> GetItems<T>(string key, string relativeUrl, CancellationToken ct = default);
}
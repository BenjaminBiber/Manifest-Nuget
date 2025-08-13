namespace BenjaminBiber.Manifest.Interfaces;

public interface ICacheService
{
    /// <summary>
    /// Ruft einen Wert aus dem Cache ab, falls vorhanden, oder fügt ihn hinzu, indem die angegebene Factory-Funktion verwendet wird.
    /// </summary>
    /// <typeparam name="T">Der Typ des Werts, der abgerufen oder hinzugefügt werden soll.</typeparam>
    /// <param name="key">Der eindeutige Schlüssel, der den Cache-Eintrag identifiziert.</param>
    /// <param name="factory">Eine Funktion, die den Wert erzeugt, falls dieser nicht im Cache vorhanden ist.</param>
    /// <param name="ttl">Die optionale Lebensdauer (Time-to-Live) des Cache-Eintrags.</param>
    /// <param name="ct">Ein Abbruch-Token, das überwacht wird, während die Operation ausgeführt wird.</param>
    /// <returns>Ein Task, der den abgerufenen oder hinzugefügten Wert enthält.</returns>
    Task<T> GetOrAddAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? ttl = null,
        CancellationToken ct = default);
    
    /// <summary>
    /// Entfernt einen Eintrag aus dem Cache anhand des angegebenen Schlüssels.
    /// </summary>
    /// <param name="key">Der Schlüssel des zu entfernenden Cache-Eintrags.</param>
    void Remove(string key);
    
    /// <summary>
    /// Löscht alle Einträge aus dem Cache.
    /// </summary>
    void Clear();
}

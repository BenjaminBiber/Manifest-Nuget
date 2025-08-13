namespace BenjaminBiber.Manifest.Models;

public class ApiResult<T>
{
    /// <summary>
    /// Gibt an, ob die Operation erfolgreich war.
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Enthält die Daten, die von der Operation zurückgegeben wurden.
    /// </summary>
    public T Data { get; set; }
    
    /// <summary>
    /// Enthält eine Fehlermeldung, falls die Operation nicht erfolgreich war.
    /// </summary>
    public string Error { get; set; }
}
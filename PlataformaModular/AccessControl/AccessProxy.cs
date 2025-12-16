namespace PlataformaAcademicaModular.AccessControl;

/// <summary>
/// Interfaz para recursos protegidos
/// </summary>
public interface ISecureResource
{
    string GetData();
    void ModifyData(string newData);
}

/// <summary>
/// Recurso real que contiene información sensible
/// </summary>
public class SecureDatabase : ISecureResource
{
    private string _data = "Información Confidencial del Sistema";

    public string GetData()
    {
        Console.WriteLine("  [REAL] Accediendo a base de datos segura...");
        return _data;
    }

    public void ModifyData(string newData)
    {
        Console.WriteLine("  [REAL] Modificando datos en base de datos segura...");
        _data = newData;
    }
}

/// <summary>
/// PATRÓN PROXY: Controla el acceso a un objeto
/// Proporciona lazy loading, control de acceso y logging
/// </summary>
public class AccessProxy : ISecureResource
{
    private SecureDatabase? _realDatabase;
    private readonly string _userRole;
    private readonly List<string> _accessLog = new();

    public AccessProxy(string userRole)
    {
        _userRole = userRole;
        Console.WriteLine($"[PROXY] Proxy creado para rol: {userRole}");
    }

    public string GetData()
    {
        LogAccess("GetData");

        if (!HasPermission("Read"))
        {
            Console.WriteLine("[PROXY] ❌ Acceso denegado: permisos insuficientes");
            return "ACCESO DENEGADO";
        }

        // Lazy initialization
        if (_realDatabase == null)
        {
            Console.WriteLine("[PROXY] Inicializando base de datos (lazy loading)...");
            _realDatabase = new SecureDatabase();
        }

        Console.WriteLine("[PROXY] ✅ Acceso concedido");
        return _realDatabase.GetData();
    }

    public void ModifyData(string newData)
    {
        LogAccess("ModifyData");

        if (!HasPermission("Write"))
        {
            Console.WriteLine("[PROXY] ❌ Modificación denegada: permisos insuficientes");
            return;
        }

        if (_realDatabase == null)
        {
            _realDatabase = new SecureDatabase();
        }

        Console.WriteLine("[PROXY] ✅ Modificación permitida");
        _realDatabase.ModifyData(newData);
    }

    private bool HasPermission(string operation)
    {
        return _userRole switch
        {
            "Administrador" => true,
            "Profesor" => operation == "Read",
            "Estudiante" => false,
            _ => false
        };
    }

    private void LogAccess(string operation)
    {
        var logEntry = $"[{DateTime.Now:HH:mm:ss}] {_userRole} intentó {operation}";
        _accessLog.Add(logEntry);
        Console.WriteLine($"[PROXY] Log: {logEntry}");
    }

    public void ShowAccessLog()
    {
        Console.WriteLine($"[PROXY] Registro de accesos ({_accessLog.Count}):");
        foreach (var log in _accessLog)
        {
            Console.WriteLine($"  {log}");
        }
    }
}

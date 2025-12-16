namespace PlataformaAcademicaModular.ResourceOptimizer;

/// <summary>
/// PATRÓN OBJECT POOL: Reutiliza objetos costosos en lugar de crearlos repetidamente
/// Mejora el rendimiento evitando la creación/destrucción constante de objetos
/// </summary>
public class ConnectionPool
{
    private readonly Queue<DatabaseConnection> _availableConnections = new();
    private readonly HashSet<DatabaseConnection> _inUseConnections = new();
    private readonly int _maxPoolSize;
    private int _currentPoolSize;
    private readonly object _lock = new();

    public ConnectionPool(int maxPoolSize = 10)
    {
        _maxPoolSize = maxPoolSize;
        Console.WriteLine($"[OBJECT POOL] Pool de conexiones creado con tamaño máximo: {maxPoolSize}");
    }

    /// <summary>
    /// Obtiene una conexión del pool (reutiliza o crea nueva)
    /// </summary>
    public DatabaseConnection AcquireConnection()
    {
        lock (_lock)
        {
            // Intentar reutilizar conexión existente
            if (_availableConnections.Count > 0)
            {
                var connection = _availableConnections.Dequeue();
                _inUseConnections.Add(connection);
                Console.WriteLine($"[OBJECT POOL] ♻️ Reutilizando conexión #{connection.Id} del pool");
                return connection;
            }

            // Crear nueva conexión si no se alcanzó el límite
            if (_currentPoolSize < _maxPoolSize)
            {
                var newConnection = new DatabaseConnection(++_currentPoolSize);
                _inUseConnections.Add(newConnection);
                Console.WriteLine($"[OBJECT POOL] ➕ Nueva conexión #{newConnection.Id} creada (Total: {_currentPoolSize}/{_maxPoolSize})");
                return newConnection;
            }

            // Pool lleno, esperar
            Console.WriteLine("[OBJECT POOL] ⚠️ Pool lleno, esperando conexión disponible...");
            throw new InvalidOperationException("Pool de conexiones lleno");
        }
    }

    /// <summary>
    /// Devuelve una conexión al pool para reutilización
    /// </summary>
    public void ReleaseConnection(DatabaseConnection connection)
    {
        lock (_lock)
        {
            if (_inUseConnections.Remove(connection))
            {
                connection.Reset();
                _availableConnections.Enqueue(connection);
                Console.WriteLine($"[OBJECT POOL] ↩️ Conexión #{connection.Id} devuelta al pool (Disponibles: {_availableConnections.Count})");
            }
        }
    }

    /// <summary>
    /// Muestra estadísticas del pool
    /// </summary>
    public void ShowStatistics()
    {
        lock (_lock)
        {
            Console.WriteLine($"\n[OBJECT POOL] 📊 Estadísticas del Pool:");
            Console.WriteLine($"  Total de conexiones: {_currentPoolSize}/{_maxPoolSize}");
            Console.WriteLine($"  En uso: {_inUseConnections.Count}");
            Console.WriteLine($"  Disponibles: {_availableConnections.Count}");
        }
    }
}

/// <summary>
/// Objeto costoso que se reutiliza (simula conexión a base de datos)
/// </summary>
public class DatabaseConnection
{
    public int Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime LastUsed { get; private set; }
    public int UsageCount { get; private set; }

    public DatabaseConnection(int id)
    {
        Id = id;
        CreatedAt = DateTime.Now;
        LastUsed = DateTime.Now;
        // Simulación de creación costosa
        Thread.Sleep(10);
    }

    public void ExecuteQuery(string query)
    {
        LastUsed = DateTime.Now;
        UsageCount++;
        Console.WriteLine($"  [Conexión #{Id}] Ejecutando: {query} (Uso #{UsageCount})");
    }

    public void Reset()
    {
        // Limpiar estado para reutilización
        Console.WriteLine($"  [Conexión #{Id}] Reseteada para reutilización");
    }
}

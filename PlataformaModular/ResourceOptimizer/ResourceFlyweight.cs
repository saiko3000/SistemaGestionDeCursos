namespace PlataformaAcademicaModular.ResourceOptimizer;

/// <summary>
/// Datos compartidos (intrínsecos) de recursos
/// </summary>
public class SharedResourceData
{
    public string Type { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public SharedResourceData(string type, string icon, string category)
    {
        Type = type;
        Icon = icon;
        Category = category;
    }
}

/// <summary>
/// PATRÓN FLYWEIGHT: Comparte eficientemente objetos que se usan en grandes cantidades
/// Minimiza el uso de memoria compartiendo datos comunes
/// </summary>
public class ResourceFlyweight
{
    private readonly SharedResourceData _sharedData;

    public ResourceFlyweight(SharedResourceData sharedData)
    {
        _sharedData = sharedData;
        Console.WriteLine($"[FLYWEIGHT] Flyweight creado para tipo: {sharedData.Type}");
    }

    public void Display(string uniqueId, string name, int size)
    {
        Console.WriteLine($"  {_sharedData.Icon} [{_sharedData.Type}] {name} (ID: {uniqueId}, {size}KB) - Categoría: {_sharedData.Category}");
    }
}

/// <summary>
/// Fábrica de Flyweights
/// </summary>
public class ResourceFlyweightFactory
{
    private readonly Dictionary<string, ResourceFlyweight> _flyweights = new();
    private int _createdCount = 0;

    public ResourceFlyweight GetFlyweight(string type, string icon, string category)
    {
        string key = $"{type}_{category}";

        if (!_flyweights.ContainsKey(key))
        {
            var sharedData = new SharedResourceData(type, icon, category);
            _flyweights[key] = new ResourceFlyweight(sharedData);
            _createdCount++;
            Console.WriteLine($"[FLYWEIGHT] Nuevo flyweight creado. Total: {_createdCount}");
        }
        else
        {
            Console.WriteLine($"[FLYWEIGHT] Reutilizando flyweight existente para {type}");
        }

        return _flyweights[key];
    }

    public void ShowStatistics()
    {
        Console.WriteLine($"\n[FLYWEIGHT] Estadísticas:");
        Console.WriteLine($"  Total de flyweights únicos: {_flyweights.Count}");
        Console.WriteLine($"  Memoria ahorrada: ~{(_createdCount - _flyweights.Count) * 100}KB (estimado)");
    }
}

/// <summary>
/// Recurso con datos únicos (extrínsecos)
/// </summary>
public class Resource
{
    private readonly string _uniqueId;
    private readonly string _name;
    private readonly int _size;
    private readonly ResourceFlyweight _flyweight;

    public Resource(string uniqueId, string name, int size, ResourceFlyweight flyweight)
    {
        _uniqueId = uniqueId;
        _name = name;
        _size = size;
        _flyweight = flyweight;
    }

    public void Display()
    {
        _flyweight.Display(_uniqueId, _name, _size);
    }
}

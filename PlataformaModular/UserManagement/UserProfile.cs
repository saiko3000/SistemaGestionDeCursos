namespace PlataformaAcademicaModular.UserManagement;

/// <summary>
/// PATRÓN PROTOTYPE: Permite clonar perfiles de usuario para crear plantillas
/// Útil para crear perfiles base que pueden ser personalizados
/// </summary>
public class UserProfile : ICloneable
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public Dictionary<string, string> Preferences { get; set; }
    public List<string> Permissions { get; set; }

    public UserProfile(string name, string email, string role)
    {
        Name = name;
        Email = email;
        Role = role;
        Preferences = new Dictionary<string, string>();
        Permissions = new List<string>();
    }

    /// <summary>
    /// Clonación superficial
    /// </summary>
    public object Clone()
    {
        Console.WriteLine($"[PROTOTYPE] Clonando perfil de {Name}");
        return MemberwiseClone();
    }

    /// <summary>
    /// Clonación profunda para copiar colecciones
    /// </summary>
    public UserProfile DeepClone()
    {
        Console.WriteLine($"[PROTOTYPE] Clonación profunda del perfil de {Name}");
        var clone = (UserProfile)MemberwiseClone();
        clone.Preferences = new Dictionary<string, string>(Preferences);
        clone.Permissions = new List<string>(Permissions);
        return clone;
    }

    public void DisplayProfile()
    {
        Console.WriteLine($"  Perfil: {Name} ({Role})");
        Console.WriteLine($"  Email: {Email}");
        Console.WriteLine($"  Permisos: {string.Join(", ", Permissions)}");
        Console.WriteLine($"  Preferencias: {Preferences.Count} configuradas");
    }
}

/// <summary>
/// Registro de prototipos predefinidos
/// </summary>
public class ProfilePrototypeRegistry
{
    private readonly Dictionary<string, UserProfile> _prototypes = new();

    public ProfilePrototypeRegistry()
    {
        // Prototipos predefinidos
        var studentTemplate = new UserProfile("Estudiante Plantilla", "student@template.com", "Estudiante");
        studentTemplate.Permissions.AddRange(new[] { "Ver Cursos", "Enviar Tareas", "Ver Calificaciones" });
        studentTemplate.Preferences["Theme"] = "Light";
        studentTemplate.Preferences["Language"] = "ES";

        var teacherTemplate = new UserProfile("Profesor Plantilla", "teacher@template.com", "Profesor");
        teacherTemplate.Permissions.AddRange(new[] { "Crear Cursos", "Calificar", "Ver Reportes", "Gestionar Alumnos" });
        teacherTemplate.Preferences["Theme"] = "Dark";
        teacherTemplate.Preferences["Language"] = "ES";

        _prototypes["student"] = studentTemplate;
        _prototypes["teacher"] = teacherTemplate;
    }

    public UserProfile GetPrototype(string key)
    {
        if (_prototypes.TryGetValue(key, out var prototype))
        {
            return prototype.DeepClone();
        }
        throw new KeyNotFoundException($"Prototipo '{key}' no encontrado");
    }

    public void RegisterPrototype(string key, UserProfile prototype)
    {
        _prototypes[key] = prototype;
        Console.WriteLine($"[PROTOTYPE] Prototipo '{key}' registrado");
    }
}

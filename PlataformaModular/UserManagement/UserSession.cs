namespace PlataformaAcademicaModular.UserManagement;

/// <summary>
/// PATRÓN SINGLETON: Garantiza una única instancia de sesión en toda la aplicación
/// Thread-safe con inicialización perezosa
/// FUNCIONAL: Gestiona registro, login y persistencia de usuarios
/// </summary>
public sealed class UserSession
{
    private static readonly Lazy<UserSession> _instance = new(() => new UserSession());
    
    // Persistencia en memoria
    private static readonly Dictionary<string, UserCredentials> _userRegistry = new();
    private static readonly List<IUser> _allUsers = new();
    
    private IUser? _currentUser;
    private DateTime _loginTime;

    private UserSession() 
    {
        // Crear usuarios por defecto
        RegisterDefaultUsers();
    }

    public static UserSession Instance => _instance.Value;

    /// <summary>
    /// Registra un nuevo usuario en el sistema
    /// </summary>
    public bool Register(string username, string password, string role, string additionalInfo)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("❌ [SINGLETON] Usuario o contraseña no pueden estar vacíos");
            return false;
        }

        if (_userRegistry.ContainsKey(username))
        {
            Console.WriteLine($"❌ [SINGLETON] El usuario '{username}' ya existe");
            return false;
        }

        // Crear usuario usando Factory
        var user = UserFactoryManager.CreateUser(role, username, $"{username}@universidad.edu", additionalInfo);
        
        // Guardar credenciales (simulación de hash)
        var credentials = new UserCredentials
        {
            Username = username,
            PasswordHash = HashPassword(password),
            Role = role,
            User = user
        };

        _userRegistry[username] = credentials;
        _allUsers.Add(user);

        Console.WriteLine($"✅ [SINGLETON] Usuario '{username}' registrado exitosamente como {role}");
        
        // Disparar notificación automática (Observer Pattern)
        try
        {
            NotificationCenter.NotificationService.Instance.NotifyUserRegistered(username, role);
        }
        catch
        {
            // Evitar errores si el servicio de notificaciones no está inicializado
        }
        
        return true;
    }

    /// <summary>
    /// Inicia sesión con validación de credenciales
    /// </summary>
    public bool Login(string username, string password)
    {
        if (!_userRegistry.TryGetValue(username, out var credentials))
        {
            Console.WriteLine($"❌ [SINGLETON] Usuario '{username}' no encontrado");
            return false;
        }

        if (credentials.PasswordHash != HashPassword(password))
        {
            Console.WriteLine("❌ [SINGLETON] Contraseña incorrecta");
            return false;
        }

        _currentUser = credentials.User;
        _loginTime = DateTime.Now;
        Console.WriteLine($"✅ [SINGLETON] Sesión iniciada para {_currentUser.Name} ({_currentUser.Role}) a las {_loginTime:HH:mm:ss}");
        
        return true;
    }

    public void Logout()
    {
        if (_currentUser != null)
        {
            Console.WriteLine($"✅ [SINGLETON] Sesión cerrada para {_currentUser.Name}");
            _currentUser = null;
        }
    }

    public IUser? CurrentUser => _currentUser;
    public bool IsLoggedIn => _currentUser != null;
    public TimeSpan SessionDuration => DateTime.Now - _loginTime;
    
    public static IReadOnlyList<IUser> GetAllUsers() => _allUsers.AsReadOnly();
    public static int GetUserCount() => _allUsers.Count;

    private string HashPassword(string password)
    {
        // Simulación simple de hash (en producción usar BCrypt o similar)
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "SALT"));
    }

    private void RegisterDefaultUsers()
    {
        // Usuarios de demostración
        Register("admin", "admin123", "administrador", "Super Admin");
        Register("profesor1", "prof123", "profesor", "Ciencias Computacionales");
        Register("estudiante1", "est123", "estudiante", "EST-2024-001");
        Register("Honorio", "12345", "estudiante", "EST-2024-002");
        Register("Noe", "12345", "profesor", "Informatica");
        Register("Benedicto", "12345", "profesor", "Informatica");
    }
}

/// <summary>
/// Credenciales de usuario para autenticación
/// </summary>
public class UserCredentials
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public IUser User { get; set; } = null!;
}

/// <summary>
/// Interfaz base para todos los usuarios
/// </summary>
public interface IUser
{
    string Name { get; }
    string Email { get; }
    string Role { get; }
    void DisplayInfo();
}

/// <summary>
/// Usuario tipo Estudiante
/// </summary>
public class Student : IUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role => "Estudiante";
    public string StudentId { get; set; }
    public List<string> EnrolledCourses { get; set; } = new();

    public Student(string name, string email, string studentId)
    {
        Name = name;
        Email = email;
        StudentId = studentId;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"  👨‍🎓 Estudiante: {Name} | ID: {StudentId} | Email: {Email}");
    }
}

/// <summary>
/// Usuario tipo Profesor
/// </summary>
public class Teacher : IUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role => "Profesor";
    public string Department { get; set; }
    public List<string> CoursesTaught { get; set; } = new();

    public Teacher(string name, string email, string department)
    {
        Name = name;
        Email = email;
        Department = department;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"  👨‍🏫 Profesor: {Name} | Departamento: {Department} | Email: {Email}");
    }
}

/// <summary>
/// Usuario tipo Administrador
/// </summary>
public class Administrator : IUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role => "Administrador";
    public string AdminLevel { get; set; }

    public Administrator(string name, string email, string adminLevel)
    {
        Name = name;
        Email = email;
        AdminLevel = adminLevel;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"  👑 Administrador: {Name} | Nivel: {AdminLevel} | Email: {Email}");
    }
}

namespace PlataformaAcademicaModular.UserManagement;

/// <summary>
/// PATRÓN FACADE: Proporciona una interfaz simplificada para un subsistema complejo
/// Oculta la complejidad de múltiples componentes detrás de una API simple
/// </summary>
public class AcademicSystemFacade
{
    private readonly UserSession _session;
    private readonly CourseBuilder.CourseManager _courseManager;
    private readonly NotificationCenter.NotificationService _notificationService;

    public AcademicSystemFacade()
    {
        _session = UserSession.Instance;
        _courseManager = CourseBuilder.CourseManager.Instance;
        _notificationService = NotificationCenter.NotificationService.Instance;
        Console.WriteLine("[FACADE] Sistema académico inicializado con interfaz simplificada");
    }

    /// <summary>
    /// Operación simplificada: Registrar e iniciar sesión en un solo paso
    /// </summary>
    public bool QuickRegisterAndLogin(string username, string password, string role, string additionalInfo)
    {
        Console.WriteLine("[FACADE] 🚀 Registro e inicio de sesión rápido...");
        
        // Paso 1: Registrar
        bool registered = _session.Register(username, password, role, additionalInfo);
        if (!registered)
        {
            return false;
        }

        // Paso 2: Iniciar sesión automáticamente
        bool loggedIn = _session.Login(username, password);
        
        if (loggedIn)
        {
            Console.WriteLine("[FACADE] ✅ Usuario registrado e iniciado sesión exitosamente");
        }

        return loggedIn;
    }

    /// <summary>
    /// Operación simplificada: Crear curso completo con configuración predeterminada
    /// </summary>
    public CourseBuilder.Course CreateQuickCourse(string courseName, string instructorName)
    {
        Console.WriteLine($"[FACADE] 🚀 Creación rápida de curso '{courseName}'...");

        var factory = new CourseBuilder.BasicContentFactory();
        var builder = new CourseBuilder.StandardCourseBuilder();

        var course = builder
            .SetBasicInfo(courseName, "", $"Curso de {courseName}")
            .SetCredits(3)
            .SetInstructor(instructorName)
            .AddTheoryContent(factory.CreateVideo())
            .AddTheoryContent(factory.CreateDocument())
            .AddExam(factory.CreateQuiz())
            .Build();

        Console.WriteLine("[FACADE] ✅ Curso creado con configuración estándar");
        return course;
    }

    /// <summary>
    /// Operación simplificada: Obtener resumen completo del sistema
    /// </summary>
    public SystemSummary GetSystemSummary()
    {
        Console.WriteLine("[FACADE] 📊 Obteniendo resumen del sistema...");

        var summary = new SystemSummary
        {
            TotalUsers = UserSession.GetUserCount(),
            TotalCourses = _courseManager.GetCourseCount(),
            TotalNotifications = _notificationService.GetNotificationCount(),
            IsUserLoggedIn = _session.IsLoggedIn,
            CurrentUserName = _session.CurrentUser?.Name ?? "Ninguno"
        };

        Console.WriteLine("[FACADE] ✅ Resumen generado");
        return summary;
    }

    /// <summary>
    /// Operación simplificada: Workflow completo de inscripción a curso
    /// </summary>
    public bool EnrollStudentInCourse(string studentUsername, string courseCode)
    {
        Console.WriteLine($"[FACADE] 🚀 Inscribiendo estudiante '{studentUsername}' en curso '{courseCode}'...");

        // Validar que el usuario actual es estudiante
        if (_session.CurrentUser?.Role != "Estudiante")
        {
            Console.WriteLine("[FACADE] ❌ Solo estudiantes pueden inscribirse en cursos");
            return false;
        }

        // Buscar curso
        var course = _courseManager.FindCourse(courseCode);
        if (course == null)
        {
            Console.WriteLine($"[FACADE] ❌ Curso '{courseCode}' no encontrado");
            return false;
        }

        // Notificar inscripción
        _notificationService.NotifyCourseCreated($"Inscripción en {course.Name}", courseCode);

        // Agregar curso a la lista del estudiante
        if (_session.CurrentUser is Student student)
        {
            if (!student.EnrolledCourses.Contains(courseCode))
            {
                student.EnrolledCourses.Add(courseCode);
                Console.WriteLine($"[FACADE] ✅ Estudiante inscrito en '{course.Name}'");
                return true;
            }
            else
            {
                Console.WriteLine($"[FACADE] ⚠️ El estudiante ya está inscrito en este curso");
                return false;
            }
        }
        
        return false;
    }
}

/// <summary>
/// Objeto de transferencia de datos para el resumen del sistema
/// </summary>
public class SystemSummary
{
    public int TotalUsers { get; set; }
    public int TotalCourses { get; set; }
    public int TotalNotifications { get; set; }
    public bool IsUserLoggedIn { get; set; }
    public string CurrentUserName { get; set; } = string.Empty;

    public void Display()
    {
        Console.WriteLine("\n📊 RESUMEN DEL SISTEMA:");
        Console.WriteLine($"  Usuarios: {TotalUsers}");
        Console.WriteLine($"  Cursos: {TotalCourses}");
        Console.WriteLine($"  Notificaciones: {TotalNotifications}");
        Console.WriteLine($"  Sesión: {(IsUserLoggedIn ? $"Activa ({CurrentUserName})" : "Inactiva")}");
    }
}

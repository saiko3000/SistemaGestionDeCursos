namespace PlataformaAcademicaModular.AccessControl;

/// <summary>
/// Solicitud de autorización
/// </summary>
public class AuthorizationRequest
{
    public string UserRole { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public bool IsAuthorized { get; set; }
}

/// <summary>
/// PATRÓN CHAIN OF RESPONSIBILITY: Procesa solicitudes a través de una cadena de manejadores
/// Cada manejador decide si procesa la solicitud o la pasa al siguiente
/// </summary>
public abstract class AuthorizationHandler
{
    protected AuthorizationHandler? _nextHandler;

    public void SetNext(AuthorizationHandler handler)
    {
        _nextHandler = handler;
    }

    public abstract void Handle(AuthorizationRequest request);
}

/// <summary>
/// Manejador: Verificación de administrador
/// </summary>
public class AdminAuthorizationHandler : AuthorizationHandler
{
    public override void Handle(AuthorizationRequest request)
    {
        if (request.UserRole == "Administrador")
        {
            request.IsAuthorized = true;
            Console.WriteLine($"[CHAIN] ✅ AdminHandler: Acceso total concedido para {request.UserRole}");
            return;
        }

        Console.WriteLine($"[CHAIN] AdminHandler: No es administrador, pasando al siguiente...");
        _nextHandler?.Handle(request);
    }
}

/// <summary>
/// Manejador: Verificación de profesor
/// </summary>
public class TeacherAuthorizationHandler : AuthorizationHandler
{
    public override void Handle(AuthorizationRequest request)
    {
        if (request.UserRole == "Profesor")
        {
            if (request.Action == "Read" || request.Action == "Create" || request.Action == "Update")
            {
                request.IsAuthorized = true;
                Console.WriteLine($"[CHAIN] ✅ TeacherHandler: Acceso concedido para {request.Action}");
                return;
            }
        }

        Console.WriteLine($"[CHAIN] TeacherHandler: No autorizado, pasando al siguiente...");
        _nextHandler?.Handle(request);
    }
}

/// <summary>
/// Manejador: Verificación de estudiante
/// </summary>
public class StudentAuthorizationHandler : AuthorizationHandler
{
    public override void Handle(AuthorizationRequest request)
    {
        if (request.UserRole == "Estudiante")
        {
            if (request.Action == "Read" && request.Resource == "Courses")
            {
                request.IsAuthorized = true;
                Console.WriteLine($"[CHAIN] ✅ StudentHandler: Acceso de lectura concedido");
                return;
            }
        }

        Console.WriteLine($"[CHAIN] StudentHandler: No autorizado, pasando al siguiente...");
        _nextHandler?.Handle(request);
    }
}

/// <summary>
/// Manejador final: Denegar por defecto
/// </summary>
public class DefaultDenyHandler : AuthorizationHandler
{
    public override void Handle(AuthorizationRequest request)
    {
        request.IsAuthorized = false;
        Console.WriteLine($"[CHAIN] ❌ DefaultDenyHandler: Acceso denegado por defecto");
    }
}

/// <summary>
/// Configurador de la cadena de autorización
/// </summary>
public class AuthorizationChain
{
    private readonly AuthorizationHandler _chain;

    public AuthorizationChain()
    {
        // Construir la cadena
        var adminHandler = new AdminAuthorizationHandler();
        var teacherHandler = new TeacherAuthorizationHandler();
        var studentHandler = new StudentAuthorizationHandler();
        var denyHandler = new DefaultDenyHandler();

        adminHandler.SetNext(teacherHandler);
        teacherHandler.SetNext(studentHandler);
        studentHandler.SetNext(denyHandler);

        _chain = adminHandler;
        Console.WriteLine("[CHAIN] Cadena de autorización configurada");
    }

    public bool Authorize(string userRole, string resource, string action)
    {
        var request = new AuthorizationRequest
        {
            UserRole = userRole,
            Resource = resource,
            Action = action
        };

        Console.WriteLine($"[CHAIN] Procesando solicitud: {userRole} -> {action} en {resource}");
        _chain.Handle(request);
        return request.IsAuthorized;
    }
}

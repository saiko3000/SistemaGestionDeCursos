namespace PlataformaAcademicaModular.UserManagement;

/// <summary>
/// PATRÓN NULL OBJECT: Proporciona un objeto que no hace nada en lugar de null
/// Evita verificaciones de null y simplifica el código cliente
/// </summary>
public interface IUserNotification
{
    void SendNotification(string message);
    void SendEmail(string subject, string body);
    void SendSMS(string phoneNumber, string message);
}

/// <summary>
/// Implementación real de notificaciones de usuario
/// </summary>
public class RealUserNotification : IUserNotification
{
    private readonly string _userName;

    public RealUserNotification(string userName)
    {
        _userName = userName;
    }

    public void SendNotification(string message)
    {
        Console.WriteLine($"[NOTIFICATION] 📧 Enviando notificación a {_userName}: {message}");
    }

    public void SendEmail(string subject, string body)
    {
        Console.WriteLine($"[EMAIL] ✉️ Enviando email a {_userName}");
        Console.WriteLine($"  Asunto: {subject}");
        Console.WriteLine($"  Cuerpo: {body}");
    }

    public void SendSMS(string phoneNumber, string message)
    {
        Console.WriteLine($"[SMS] 📱 Enviando SMS a {_userName} ({phoneNumber}): {message}");
    }
}

/// <summary>
/// NULL OBJECT: No hace nada, pero cumple la interfaz
/// Evita verificaciones de null en el código cliente
/// </summary>
public class NullUserNotification : IUserNotification
{
    public void SendNotification(string message)
    {
        // No hace nada - usuario sin notificaciones habilitadas
        Console.WriteLine("[NULL OBJECT] Usuario sin notificaciones habilitadas - mensaje ignorado");
    }

    public void SendEmail(string subject, string body)
    {
        // No hace nada
        Console.WriteLine("[NULL OBJECT] Usuario sin email configurado - email ignorado");
    }

    public void SendSMS(string phoneNumber, string message)
    {
        // No hace nada
        Console.WriteLine("[NULL OBJECT] Usuario sin SMS habilitado - mensaje ignorado");
    }
}

/// <summary>
/// Servicio que utiliza el patrón Null Object
/// </summary>
public class UserNotificationService
{
    private readonly Dictionary<string, IUserNotification> _userNotifications = new();

    public void RegisterUser(string userName, bool enableNotifications)
    {
        if (enableNotifications)
        {
            _userNotifications[userName] = new RealUserNotification(userName);
            Console.WriteLine($"[NULL OBJECT] Usuario '{userName}' registrado con notificaciones ACTIVAS");
        }
        else
        {
            _userNotifications[userName] = new NullUserNotification();
            Console.WriteLine($"[NULL OBJECT] Usuario '{userName}' registrado con notificaciones DESACTIVADAS (Null Object)");
        }
    }

    public void NotifyUser(string userName, string message)
    {
        // No necesita verificar null - siempre hay un objeto
        var notification = _userNotifications.TryGetValue(userName, out var notif) 
            ? notif 
            : new NullUserNotification();

        notification.SendNotification(message);
    }

    public void SendEmailToUser(string userName, string subject, string body)
    {
        var notification = _userNotifications.TryGetValue(userName, out var notif) 
            ? notif 
            : new NullUserNotification();

        notification.SendEmail(subject, body);
    }
}

/// <summary>
/// Ejemplo de uso del patrón Null Object en el contexto académico
/// </summary>
public class NullCourse : CourseBuilder.Course
{
    public NullCourse()
    {
        Name = "Curso No Disponible";
        Code = "N/A";
        Description = "Este curso no existe o no está disponible";
        Credits = 0;
        Instructor = "N/A";
    }

    public new void DisplayCourse()
    {
        Console.WriteLine("[NULL OBJECT] ⚠️ Curso no disponible - mostrando información por defecto");
        base.DisplayCourse();
    }
}

/// <summary>
/// Manager que utiliza Null Object para cursos
/// </summary>
public static class SafeCourseManager
{
    public static CourseBuilder.Course GetCourseOrNull(string courseCode)
    {
        var course = CourseBuilder.CourseManager.Instance.FindCourse(courseCode);
        
        if (course == null)
        {
            Console.WriteLine($"[NULL OBJECT] Curso '{courseCode}' no encontrado - devolviendo Null Object");
            return new NullCourse();
        }

        return course;
    }
}

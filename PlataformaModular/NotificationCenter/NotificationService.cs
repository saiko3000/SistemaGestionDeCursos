namespace PlataformaAcademicaModular.NotificationCenter;

/// <summary>
/// PATRÓN SINGLETON: Servicio centralizado de notificaciones
/// Dispara notificaciones automáticas en eventos del sistema
/// </summary>
public sealed class NotificationService
{
    private static readonly Lazy<NotificationService> _instance = new(() => new NotificationService());
    private readonly NotificationSubject _subject;
    private readonly List<Notification> _notificationHistory = new();

    private NotificationService()
    {
        _subject = new NotificationSubject();
        
        // Suscribir observadores por defecto
        _subject.Attach(new UserObserver("Sistema"));
        _subject.Attach(new EmailObserver());
        _subject.Attach(new LoggerObserver());
        
        Console.WriteLine("🔔 [NOTIFICATION SERVICE] Servicio de notificaciones inicializado");
    }

    public static NotificationService Instance => _instance.Value;

    /// <summary>
    /// Notifica cuando se registra un nuevo usuario
    /// </summary>
    public void NotifyUserRegistered(string username, string role)
    {
        var notification = new Notification
        {
            Title = "Nuevo Usuario Registrado",
            Message = $"El usuario '{username}' se ha registrado como {role}",
            Timestamp = DateTime.Now,
            Priority = "Normal"
        };

        SendNotification(notification);
    }

    /// <summary>
    /// Notifica cuando se crea un nuevo curso
    /// </summary>
    public void NotifyCourseCreated(string courseName, string courseCode)
    {
        var notification = new Notification
        {
            Title = "Nuevo Curso Disponible",
            Message = $"El curso '{courseName}' ({courseCode}) está ahora disponible para inscripción",
            Timestamp = DateTime.Now,
            Priority = "Alta"
        };

        SendNotification(notification);
    }

    /// <summary>
    /// Notifica inicio de sesión
    /// </summary>
    public void NotifyUserLogin(string username, string role)
    {
        var notification = new Notification
        {
            Title = "Inicio de Sesión",
            Message = $"{username} ({role}) ha iniciado sesión",
            Timestamp = DateTime.Now,
            Priority = "Baja"
        };

        SendNotification(notification);
    }

    /// <summary>
    /// Envía una notificación y la guarda en el historial
    /// </summary>
    private void SendNotification(Notification notification)
    {
        Console.WriteLine($"\n🔔 [NOTIFICACIÓN] {notification.Title}");
        _subject.Notify(notification);
        _notificationHistory.Add(notification);
    }

    /// <summary>
    /// Muestra el historial de notificaciones
    /// </summary>
    public void DisplayNotificationHistory()
    {
        if (_notificationHistory.Count == 0)
        {
            Console.WriteLine("\n📭 No hay notificaciones");
            return;
        }

        Console.WriteLine($"\n📬 HISTORIAL DE NOTIFICACIONES ({_notificationHistory.Count}):");
        Console.WriteLine(new string('─', 70));

        for (int i = _notificationHistory.Count - 1; i >= 0 && i >= _notificationHistory.Count - 10; i--)
        {
            var notif = _notificationHistory[i];
            Console.WriteLine($"  [{notif.Timestamp:HH:mm:ss}] [{notif.Priority}] {notif.Title}");
            Console.WriteLine($"      {notif.Message}");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Obtiene el conteo de notificaciones
    /// </summary>
    public int GetNotificationCount() => _notificationHistory.Count;
}

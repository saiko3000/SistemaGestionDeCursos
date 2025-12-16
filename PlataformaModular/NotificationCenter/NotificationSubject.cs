namespace PlataformaAcademicaModular.NotificationCenter;

/// <summary>
/// Datos de la notificación
/// </summary>
public class Notification
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Priority { get; set; } = "Normal";
}

/// <summary>
/// Interfaz para observadores de notificaciones
/// </summary>
public interface INotificationObserver
{
    void Update(Notification notification);
    string ObserverName { get; }
}

/// <summary>
/// PATRÓN OBSERVER: Implementa el mecanismo de suscripción/notificación
/// Permite que múltiples observadores reaccionen a eventos
/// </summary>
public class NotificationSubject
{
    private readonly List<INotificationObserver> _observers = new();

    public void Attach(INotificationObserver observer)
    {
        _observers.Add(observer);
        Console.WriteLine($"[OBSERVER] {observer.ObserverName} suscrito a notificaciones");
    }

    public void Detach(INotificationObserver observer)
    {
        _observers.Remove(observer);
        Console.WriteLine($"[OBSERVER] {observer.ObserverName} desuscrito de notificaciones");
    }

    public void Notify(Notification notification)
    {
        Console.WriteLine($"[OBSERVER] Notificando a {_observers.Count} observadores...");
        foreach (var observer in _observers)
        {
            observer.Update(notification);
        }
    }
}

/// <summary>
/// Observador concreto: Usuario
/// </summary>
public class UserObserver : INotificationObserver
{
    public string ObserverName { get; }

    public UserObserver(string name)
    {
        ObserverName = name;
    }

    public void Update(Notification notification)
    {
        Console.WriteLine($"  📧 {ObserverName} recibió: [{notification.Priority}] {notification.Title}");
    }
}

/// <summary>
/// Observador concreto: Sistema de Email
/// </summary>
public class EmailObserver : INotificationObserver
{
    public string ObserverName => "Sistema de Email";

    public void Update(Notification notification)
    {
        Console.WriteLine($"  ✉️ Email enviado: {notification.Title} - {notification.Message}");
    }
}

/// <summary>
/// Observador concreto: Logger
/// </summary>
public class LoggerObserver : INotificationObserver
{
    public string ObserverName => "Sistema de Logs";

    public void Update(Notification notification)
    {
        Console.WriteLine($"  📝 Log registrado: [{notification.Timestamp:HH:mm:ss}] {notification.Title}");
    }
}

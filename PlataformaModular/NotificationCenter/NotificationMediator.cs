namespace PlataformaAcademicaModular.NotificationCenter;

/// <summary>
/// Componente base del sistema de notificaciones
/// </summary>
public abstract class NotificationComponent
{
    protected INotificationMediator? Mediator;

    public void SetMediator(INotificationMediator mediator)
    {
        Mediator = mediator;
    }
}

/// <summary>
/// PATRÓN MEDIATOR: Centraliza la comunicación entre componentes
/// Reduce el acoplamiento entre objetos que interactúan
/// </summary>
public interface INotificationMediator
{
    void Notify(object sender, string eventType, object? data = null);
}

/// <summary>
/// Mediador concreto para el sistema de notificaciones
/// </summary>
public class NotificationMediator : INotificationMediator
{
    private readonly NotificationSubject _subject;
    private readonly NotificationQueue _queue;
    private readonly NotificationLogger _logger;

    public NotificationMediator(NotificationSubject subject, NotificationQueue queue, NotificationLogger logger)
    {
        _subject = subject;
        _queue = queue;
        _logger = logger;

        // Solo los componentes que heredan de NotificationComponent tienen SetMediator
        _queue.SetMediator(this);
        _logger.SetMediator(this);
    }

    public void Notify(object sender, string eventType, object? data = null)
    {
        Console.WriteLine($"[MEDIATOR] Evento '{eventType}' recibido de {sender.GetType().Name}");

        switch (eventType)
        {
            case "NotificationCreated":
                if (data is Notification notification)
                {
                    _logger.Log($"Nueva notificación: {notification.Title}");
                    _queue.Enqueue(notification);
                    _subject.Notify(notification);
                }
                break;

            case "NotificationProcessed":
                _logger.Log("Notificación procesada exitosamente");
                break;

            case "QueueEmpty":
                _logger.Log("Cola de notificaciones vacía");
                break;
        }
    }
}

/// <summary>
/// Componente: Cola de notificaciones
/// </summary>
public class NotificationQueue : NotificationComponent
{
    private readonly Queue<Notification> _queue = new();

    public void Enqueue(Notification notification)
    {
        _queue.Enqueue(notification);
        Console.WriteLine($"[MEDIATOR] Notificación encolada. Total en cola: {_queue.Count}");
    }

    public Notification? Dequeue()
    {
        if (_queue.Count > 0)
        {
            var notification = _queue.Dequeue();
            Mediator?.Notify(this, "NotificationProcessed");
            return notification;
        }
        Mediator?.Notify(this, "QueueEmpty");
        return null;
    }
}

/// <summary>
/// Componente: Logger de notificaciones
/// </summary>
public class NotificationLogger : NotificationComponent
{
    private readonly List<string> _logs = new();

    public void Log(string message)
    {
        var logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";
        _logs.Add(logEntry);
        Console.WriteLine($"[MEDIATOR] Log: {message}");
    }

    public IReadOnlyList<string> GetLogs() => _logs.AsReadOnly();
}

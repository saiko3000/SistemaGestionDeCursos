namespace PlataformaAcademicaModular.NotificationCenter;

/// <summary>
/// PATRÓN COMMAND: Encapsula una solicitud como un objeto
/// Permite parametrizar clientes con diferentes solicitudes, encolar y deshacer operaciones
/// </summary>
public interface INotificationCommand
{
    void Execute();
    void Undo();
    string Description { get; }
}

/// <summary>
/// Comando concreto: Enviar notificación
/// </summary>
public class SendNotificationCommand : INotificationCommand
{
    private readonly NotificationSubject _subject;
    private readonly Notification _notification;
    private bool _executed;

    public string Description => $"Enviar: {_notification.Title}";

    public SendNotificationCommand(NotificationSubject subject, Notification notification)
    {
        _subject = subject;
        _notification = notification;
    }

    public void Execute()
    {
        Console.WriteLine($"[COMMAND] Ejecutando: {Description}");
        _subject.Notify(_notification);
        _executed = true;
    }

    public void Undo()
    {
        if (_executed)
        {
            Console.WriteLine($"[COMMAND] Deshaciendo: {Description}");
            // En un sistema real, aquí se revertiría la notificación
            _executed = false;
        }
    }
}

/// <summary>
/// Comando concreto: Suscribir observador
/// </summary>
public class SubscribeCommand : INotificationCommand
{
    private readonly NotificationSubject _subject;
    private readonly INotificationObserver _observer;

    public string Description => $"Suscribir: {_observer.ObserverName}";

    public SubscribeCommand(NotificationSubject subject, INotificationObserver observer)
    {
        _subject = subject;
        _observer = observer;
    }

    public void Execute()
    {
        Console.WriteLine($"[COMMAND] Ejecutando: {Description}");
        _subject.Attach(_observer);
    }

    public void Undo()
    {
        Console.WriteLine($"[COMMAND] Deshaciendo: {Description}");
        _subject.Detach(_observer);
    }
}

/// <summary>
/// Comando concreto: Desuscribir observador
/// </summary>
public class UnsubscribeCommand : INotificationCommand
{
    private readonly NotificationSubject _subject;
    private readonly INotificationObserver _observer;

    public string Description => $"Desuscribir: {_observer.ObserverName}";

    public UnsubscribeCommand(NotificationSubject subject, INotificationObserver observer)
    {
        _subject = subject;
        _observer = observer;
    }

    public void Execute()
    {
        Console.WriteLine($"[COMMAND] Ejecutando: {Description}");
        _subject.Detach(_observer);
    }

    public void Undo()
    {
        Console.WriteLine($"[COMMAND] Deshaciendo: {Description}");
        _subject.Attach(_observer);
    }
}

/// <summary>
/// Invocador de comandos con historial
/// </summary>
public class NotificationInvoker
{
    private readonly Stack<INotificationCommand> _commandHistory = new();

    public void ExecuteCommand(INotificationCommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
    }

    public void UndoLastCommand()
    {
        if (_commandHistory.Count > 0)
        {
            var command = _commandHistory.Pop();
            command.Undo();
        }
        else
        {
            Console.WriteLine("[COMMAND] No hay comandos para deshacer");
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine($"[COMMAND] Historial ({_commandHistory.Count} comandos):");
        foreach (var cmd in _commandHistory)
        {
            Console.WriteLine($"  - {cmd.Description}");
        }
    }
}

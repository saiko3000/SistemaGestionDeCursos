namespace PlataformaAcademicaModular.AccessControl;

/// <summary>
/// Contexto de sesión
/// </summary>
public class SessionContext
{
    private ISessionState _state;

    public SessionContext()
    {
        // Estado inicial
        _state = new IdleState();
        Console.WriteLine("[STATE] Sesión creada en estado Idle");
    }

    public void SetState(ISessionState state)
    {
        _state = state;
        Console.WriteLine($"[STATE] Estado cambiado a: {state.GetType().Name}");
    }

    public void Login()
    {
        _state.Login(this);
    }

    public void Activity()
    {
        _state.Activity(this);
    }

    public void Timeout()
    {
        _state.Timeout(this);
    }

    public void Logout()
    {
        _state.Logout(this);
    }

    public string GetCurrentState()
    {
        return _state.GetType().Name;
    }
}

/// <summary>
/// PATRÓN STATE: Permite que un objeto altere su comportamiento cuando su estado interno cambia
/// El objeto parecerá cambiar de clase
/// </summary>
public interface ISessionState
{
    void Login(SessionContext context);
    void Activity(SessionContext context);
    void Timeout(SessionContext context);
    void Logout(SessionContext context);
}

/// <summary>
/// Estado: Sesión inactiva
/// </summary>
public class IdleState : ISessionState
{
    public void Login(SessionContext context)
    {
        Console.WriteLine("[STATE] IdleState: Iniciando sesión...");
        context.SetState(new ActiveState());
    }

    public void Activity(SessionContext context)
    {
        Console.WriteLine("[STATE] IdleState: No hay sesión activa");
    }

    public void Timeout(SessionContext context)
    {
        Console.WriteLine("[STATE] IdleState: Ya está inactivo");
    }

    public void Logout(SessionContext context)
    {
        Console.WriteLine("[STATE] IdleState: No hay sesión para cerrar");
    }
}

/// <summary>
/// Estado: Sesión activa
/// </summary>
public class ActiveState : ISessionState
{
    private DateTime _lastActivity = DateTime.Now;

    public void Login(SessionContext context)
    {
        Console.WriteLine("[STATE] ActiveState: Ya hay una sesión activa");
    }

    public void Activity(SessionContext context)
    {
        _lastActivity = DateTime.Now;
        Console.WriteLine($"[STATE] ActiveState: Actividad registrada a las {_lastActivity:HH:mm:ss}");
    }

    public void Timeout(SessionContext context)
    {
        Console.WriteLine("[STATE] ActiveState: Sesión expirada por inactividad");
        context.SetState(new ExpiredState());
    }

    public void Logout(SessionContext context)
    {
        Console.WriteLine("[STATE] ActiveState: Cerrando sesión...");
        context.SetState(new IdleState());
    }
}

/// <summary>
/// Estado: Sesión expirada
/// </summary>
public class ExpiredState : ISessionState
{
    public void Login(SessionContext context)
    {
        Console.WriteLine("[STATE] ExpiredState: Reiniciando sesión...");
        context.SetState(new ActiveState());
    }

    public void Activity(SessionContext context)
    {
        Console.WriteLine("[STATE] ExpiredState: Sesión expirada, debe iniciar sesión nuevamente");
    }

    public void Timeout(SessionContext context)
    {
        Console.WriteLine("[STATE] ExpiredState: La sesión ya está expirada");
    }

    public void Logout(SessionContext context)
    {
        Console.WriteLine("[STATE] ExpiredState: Limpiando sesión expirada...");
        context.SetState(new IdleState());
    }
}

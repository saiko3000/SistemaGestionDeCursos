namespace PlataformaAcademicaModular.UIAdapter;

/// <summary>
/// Abstracción de UI
/// </summary>
public abstract class UIAbstraction
{
    protected IUIRenderer _renderer;

    protected UIAbstraction(IUIRenderer renderer)
    {
        _renderer = renderer;
    }

    public abstract void Display(string content);
}

/// <summary>
/// PATRÓN BRIDGE: Separa una abstracción de su implementación
/// Permite que ambas varíen independientemente
/// </summary>
public interface IUIRenderer
{
    void Render(string content, string format);
}

/// <summary>
/// Implementación concreta: Renderizador de texto
/// </summary>
public class TextRenderer : IUIRenderer
{
    public void Render(string content, string format)
    {
        Console.WriteLine($"[BRIDGE-TextRenderer] {format}: {content}");
    }
}

/// <summary>
/// Implementación concreta: Renderizador HTML (simulado en consola)
/// </summary>
public class HtmlRenderer : IUIRenderer
{
    public void Render(string content, string format)
    {
        Console.WriteLine($"[BRIDGE-HtmlRenderer] <{format}>{content}</{format}>");
    }
}

/// <summary>
/// Abstracción refinada: Mensaje simple
/// </summary>
public class SimpleMessage : UIAbstraction
{
    public SimpleMessage(IUIRenderer renderer) : base(renderer) { }

    public override void Display(string content)
    {
        Console.WriteLine("[BRIDGE] Mostrando mensaje simple");
        _renderer.Render(content, "p");
    }
}

/// <summary>
/// Abstracción refinada: Alerta
/// </summary>
public class AlertMessage : UIAbstraction
{
    public AlertMessage(IUIRenderer renderer) : base(renderer) { }

    public override void Display(string content)
    {
        Console.WriteLine("[BRIDGE] Mostrando alerta");
        _renderer.Render($"⚠️ {content}", "alert");
    }
}

/// <summary>
/// Abstracción refinada: Título
/// </summary>
public class TitleMessage : UIAbstraction
{
    public TitleMessage(IUIRenderer renderer) : base(renderer) { }

    public override void Display(string content)
    {
        Console.WriteLine("[BRIDGE] Mostrando título");
        _renderer.Render(content.ToUpper(), "h1");
    }
}

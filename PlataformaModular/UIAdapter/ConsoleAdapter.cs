namespace PlataformaAcademicaModular.UIAdapter;

/// <summary>
/// Interfaz moderna de UI
/// </summary>
public interface IModernUI
{
    void RenderElement(string content, string style);
}

/// <summary>
/// Sistema legacy de consola
/// </summary>
public class LegacyConsoleSystem
{
    public void PrintText(string text)
    {
        Console.WriteLine($"[LEGACY] {text}");
    }

    public void PrintWithColor(string text, ConsoleColor color)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine($"[LEGACY] {text}");
        Console.ForegroundColor = oldColor;
    }
}

/// <summary>
/// PATRÓN ADAPTER: Convierte la interfaz de una clase en otra que los clientes esperan
/// Permite que clases con interfaces incompatibles trabajen juntas
/// </summary>
public class ConsoleAdapter : IModernUI
{
    private readonly LegacyConsoleSystem _legacySystem;

    public ConsoleAdapter(LegacyConsoleSystem legacySystem)
    {
        _legacySystem = legacySystem;
        Console.WriteLine("[ADAPTER] Adaptador de consola creado");
    }

    public void RenderElement(string content, string style)
    {
        Console.WriteLine($"[ADAPTER] Adaptando renderizado con estilo '{style}'");
        
        switch (style.ToLower())
        {
            case "error":
                _legacySystem.PrintWithColor(content, ConsoleColor.Red);
                break;
            case "success":
                _legacySystem.PrintWithColor(content, ConsoleColor.Green);
                break;
            case "warning":
                _legacySystem.PrintWithColor(content, ConsoleColor.Yellow);
                break;
            case "info":
                _legacySystem.PrintWithColor(content, ConsoleColor.Cyan);
                break;
            default:
                _legacySystem.PrintText(content);
                break;
        }
    }
}

namespace PlataformaAcademicaModular.UIAdapter;

/// <summary>
/// Componente base de UI
/// </summary>
public interface IUIElement
{
    void Display();
}

/// <summary>
/// Componente concreto: Texto simple
/// </summary>
public class SimpleText : IUIElement
{
    private readonly string _text;

    public SimpleText(string text)
    {
        _text = text;
    }

    public void Display()
    {
        Console.WriteLine(_text);
    }
}

/// <summary>
/// PATRÓN DECORATOR: Añade responsabilidades adicionales a un objeto dinámicamente
/// Proporciona una alternativa flexible a la herencia para extender funcionalidad
/// </summary>
public abstract class UIDecorator : IUIElement
{
    protected IUIElement _component;

    protected UIDecorator(IUIElement component)
    {
        _component = component;
    }

    public virtual void Display()
    {
        _component.Display();
    }
}

/// <summary>
/// Decorador concreto: Borde
/// </summary>
public class BorderDecorator : UIDecorator
{
    private readonly string _borderChar;

    public BorderDecorator(IUIElement component, string borderChar = "═") : base(component)
    {
        _borderChar = borderChar;
    }

    public override void Display()
    {
        Console.WriteLine($"[DECORATOR] Aplicando borde");
        Console.WriteLine(new string(_borderChar[0], 50));
        _component.Display();
        Console.WriteLine(new string(_borderChar[0], 50));
    }
}

/// <summary>
/// Decorador concreto: Color
/// </summary>
public class ColorDecorator : UIDecorator
{
    private readonly ConsoleColor _color;

    public ColorDecorator(IUIElement component, ConsoleColor color) : base(component)
    {
        _color = color;
    }

    public override void Display()
    {
        Console.WriteLine($"[DECORATOR] Aplicando color {_color}");
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = _color;
        _component.Display();
        Console.ForegroundColor = oldColor;
    }
}

/// <summary>
/// Decorador concreto: Padding
/// </summary>
public class PaddingDecorator : UIDecorator
{
    private readonly int _padding;

    public PaddingDecorator(IUIElement component, int padding) : base(component)
    {
        _padding = padding;
    }

    public override void Display()
    {
        Console.WriteLine($"[DECORATOR] Aplicando padding de {_padding} espacios");
        var indent = new string(' ', _padding);
        Console.Write(indent);
        _component.Display();
    }
}

/// <summary>
/// Decorador concreto: Icono
/// </summary>
public class IconDecorator : UIDecorator
{
    private readonly string _icon;

    public IconDecorator(IUIElement component, string icon) : base(component)
    {
        _icon = icon;
    }

    public override void Display()
    {
        Console.WriteLine($"[DECORATOR] Aplicando icono {_icon}");
        Console.Write($"{_icon} ");
        _component.Display();
    }
}

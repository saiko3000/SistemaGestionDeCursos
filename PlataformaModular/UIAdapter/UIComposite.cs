namespace PlataformaAcademicaModular.UIAdapter;

/// <summary>
/// PATRÓN COMPOSITE: Compone objetos en estructuras de árbol
/// Permite tratar objetos individuales y composiciones de manera uniforme
/// </summary>
public abstract class UIComponent
{
    protected string _name;

    protected UIComponent(string name)
    {
        _name = name;
    }

    public abstract void Render(int depth = 0);
    
    public virtual void Add(UIComponent component)
    {
        throw new NotSupportedException();
    }

    public virtual void Remove(UIComponent component)
    {
        throw new NotSupportedException();
    }
}

/// <summary>
/// Hoja: Elemento simple de UI
/// </summary>
public class UIElement : UIComponent
{
    private readonly string _content;

    public UIElement(string name, string content) : base(name)
    {
        _content = content;
    }

    public override void Render(int depth = 0)
    {
        var indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}[COMPOSITE-Element] {_name}: {_content}");
    }
}

/// <summary>
/// Compuesto: Contenedor de elementos UI
/// </summary>
public class UIContainer : UIComponent
{
    private readonly List<UIComponent> _children = new();

    public UIContainer(string name) : base(name) { }

    public override void Add(UIComponent component)
    {
        _children.Add(component);
        Console.WriteLine($"[COMPOSITE] Componente agregado a {_name}");
    }

    public override void Remove(UIComponent component)
    {
        _children.Remove(component);
        Console.WriteLine($"[COMPOSITE] Componente removido de {_name}");
    }

    public override void Render(int depth = 0)
    {
        var indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}[COMPOSITE-Container] {_name} ({_children.Count} hijos)");
        
        foreach (var child in _children)
        {
            child.Render(depth + 1);
        }
    }
}

/// <summary>
/// Ejemplo de uso: Panel complejo
/// </summary>
public class UIPanel : UIContainer
{
    public UIPanel(string name) : base(name) { }

    public override void Render(int depth = 0)
    {
        var indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}╔══════════════════════════════╗");
        Console.WriteLine($"{indent}║ PANEL: {_name.PadRight(20)} ║");
        Console.WriteLine($"{indent}╠══════════════════════════════╣");
        
        foreach (var child in _children)
        {
            child.Render(depth + 1);
        }
        
        Console.WriteLine($"{indent}╚══════════════════════════════╝");
    }

    private readonly List<UIComponent> _children = new();

    public override void Add(UIComponent component)
    {
        _children.Add(component);
    }
}

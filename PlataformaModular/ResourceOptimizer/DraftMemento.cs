namespace PlataformaAcademicaModular.ResourceOptimizer;

/// <summary>
/// PATRÓN MEMENTO: Captura y externaliza el estado interno de un objeto
/// Permite restaurar el objeto a ese estado más tarde
/// </summary>
public class DraftMemento
{
    private readonly string _content;
    private readonly DateTime _timestamp;
    private readonly string _author;

    public DraftMemento(string content, string author)
    {
        _content = content;
        _author = author;
        _timestamp = DateTime.Now;
        Console.WriteLine($"[MEMENTO] Memento creado a las {_timestamp:HH:mm:ss}");
    }

    public string GetContent() => _content;
    public DateTime GetTimestamp() => _timestamp;
    public string GetAuthor() => _author;

    public void ShowInfo()
    {
        Console.WriteLine($"  Guardado: {_timestamp:dd/MM/yyyy HH:mm:ss} por {_author}");
        Console.WriteLine($"  Contenido: {_content.Substring(0, Math.Min(50, _content.Length))}...");
    }
}

/// <summary>
/// Originador: Borrador de curso
/// </summary>
public class CourseDraft
{
    private string _content;
    private string _author;

    public CourseDraft(string author)
    {
        _author = author;
        _content = string.Empty;
    }

    public void Write(string text)
    {
        _content += text;
        Console.WriteLine($"[MEMENTO] Contenido actualizado. Longitud: {_content.Length} caracteres");
    }

    public void SetContent(string content)
    {
        _content = content;
        Console.WriteLine($"[MEMENTO] Contenido establecido directamente");
    }

    public DraftMemento Save()
    {
        Console.WriteLine("[MEMENTO] Guardando estado actual...");
        return new DraftMemento(_content, _author);
    }

    public void Restore(DraftMemento memento)
    {
        _content = memento.GetContent();
        Console.WriteLine($"[MEMENTO] Estado restaurado desde {memento.GetTimestamp():HH:mm:ss}");
    }

    public void Display()
    {
        Console.WriteLine($"\n📝 Borrador de {_author}:");
        Console.WriteLine($"   {_content}");
    }
}

/// <summary>
/// Cuidador: Gestiona el historial de mementos
/// </summary>
public class DraftHistory
{
    private readonly Stack<DraftMemento> _history = new();
    private readonly CourseDraft _draft;

    public DraftHistory(CourseDraft draft)
    {
        _draft = draft;
    }

    public void Backup()
    {
        Console.WriteLine("[MEMENTO] Creando punto de restauración...");
        _history.Push(_draft.Save());
    }

    public void Undo()
    {
        if (_history.Count > 0)
        {
            var memento = _history.Pop();
            Console.WriteLine("[MEMENTO] Deshaciendo cambios...");
            _draft.Restore(memento);
        }
        else
        {
            Console.WriteLine("[MEMENTO] No hay estados anteriores para restaurar");
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine($"\n[MEMENTO] Historial ({_history.Count} versiones guardadas):");
        int version = _history.Count;
        foreach (var memento in _history)
        {
            Console.WriteLine($"\n  Versión {version--}:");
            memento.ShowInfo();
        }
    }
}

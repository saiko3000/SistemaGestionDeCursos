namespace PlataformaAcademicaModular.BehaviorExtras;

/// <summary>
/// Colección de estudiantes
/// </summary>
public class StudentCollection
{
    private readonly List<string> _students = new();

    public void AddStudent(string name)
    {
        _students.Add(name);
    }

    public ICustomIterator CreateIterator()
    {
        return new StudentIterator(_students);
    }

    public int Count => _students.Count;
}

/// <summary>
/// PATRÓN ITERATOR: Proporciona una forma de acceder secuencialmente a elementos
/// Sin exponer la representación subyacente
/// </summary>
public interface ICustomIterator
{
    bool HasNext();
    string Next();
    void Reset();
}

/// <summary>
/// Iterador concreto para estudiantes
/// </summary>
public class StudentIterator : ICustomIterator
{
    private readonly List<string> _students;
    private int _position = 0;

    public StudentIterator(List<string> students)
    {
        _students = new List<string>(students);
        Console.WriteLine($"[ITERATOR] Iterador creado para {_students.Count} estudiantes");
    }

    public bool HasNext()
    {
        return _position < _students.Count;
    }

    public string Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("No hay más elementos");
        }

        var student = _students[_position];
        _position++;
        Console.WriteLine($"[ITERATOR] Elemento {_position}/{_students.Count}: {student}");
        return student;
    }

    public void Reset()
    {
        _position = 0;
        Console.WriteLine("[ITERATOR] Iterador reiniciado");
    }
}

/// <summary>
/// Iterador con filtro
/// </summary>
public class FilteredIterator : ICustomIterator
{
    private readonly List<string> _filteredStudents;
    private int _position = 0;

    public FilteredIterator(List<string> students, Func<string, bool> filter)
    {
        _filteredStudents = students.Where(filter).ToList();
        Console.WriteLine($"[ITERATOR] Iterador filtrado creado: {_filteredStudents.Count} elementos");
    }

    public bool HasNext() => _position < _filteredStudents.Count;

    public string Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("No hay más elementos");
        }
        return _filteredStudents[_position++];
    }

    public void Reset() => _position = 0;
}

namespace PlataformaAcademicaModular.CourseBuilder;

/// <summary>
/// PATRÓN SINGLETON: Gestor centralizado de cursos
/// Proporciona persistencia en memoria y operaciones CRUD
/// </summary>
public sealed class CourseManager
{
    private static readonly Lazy<CourseManager> _instance = new(() => new CourseManager());
    private static readonly List<Course> _courses = new();
    private static int _nextCourseId = 1;

    private CourseManager() { }

    public static CourseManager Instance => _instance.Value;

    /// <summary>
    /// Guarda un curso en el repositorio
    /// </summary>
    public void SaveCourse(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.Code))
        {
            course.Code = $"COURSE-{_nextCourseId++:D3}";
        }

        _courses.Add(course);
        Console.WriteLine($"✅ [COURSE MANAGER] Curso '{course.Name}' guardado con código {course.Code}");
    }

    /// <summary>
    /// Obtiene todos los cursos disponibles
    /// </summary>
    public IReadOnlyList<Course> GetAllCourses() => _courses.AsReadOnly();

    /// <summary>
    /// Busca un curso por código
    /// </summary>
    public Course? FindCourse(string code)
    {
        return _courses.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Obtiene el total de cursos
    /// </summary>
    public int GetCourseCount() => _courses.Count;

    /// <summary>
    /// Muestra todos los cursos disponibles
    /// </summary>
    public void DisplayAllCourses()
    {
        if (_courses.Count == 0)
        {
            Console.WriteLine("📚 No hay cursos disponibles");
            return;
        }

        Console.WriteLine($"\n📚 CURSOS DISPONIBLES ({_courses.Count}):");
        Console.WriteLine(new string('─', 70));
        
        foreach (var course in _courses)
        {
            Console.WriteLine($"  [{course.Code}] {course.Name}");
            Console.WriteLine($"      Instructor: {course.Instructor} | Créditos: {course.Credits}");
            Console.WriteLine($"      Contenido: {course.TheoryContent.Count} teoría, {course.PracticeContent.Count} práctica, {course.Exams.Count} exámenes");
            Console.WriteLine();
        }
    }
}

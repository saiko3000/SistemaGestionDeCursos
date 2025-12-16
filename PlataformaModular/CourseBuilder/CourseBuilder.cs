namespace PlataformaAcademicaModular.CourseBuilder;

/// <summary>
/// Clase que representa un curso completo
/// </summary>
public class Course
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<IContent> TheoryContent { get; set; } = new();
    public List<IContent> PracticeContent { get; set; } = new();
    public List<IContent> Exams { get; set; } = new();
    public int Credits { get; set; }
    public string Instructor { get; set; } = string.Empty;

    public void DisplayCourse()
    {
        Console.WriteLine($"\n📚 Curso: {Name} ({Code})");
        Console.WriteLine($"   Descripción: {Description}");
        Console.WriteLine($"   Créditos: {Credits} | Instructor: {Instructor}");
        Console.WriteLine($"   Contenido Teórico: {TheoryContent.Count} elementos");
        Console.WriteLine($"   Contenido Práctico: {PracticeContent.Count} elementos");
        Console.WriteLine($"   Exámenes: {Exams.Count} elementos");
    }
}

/// <summary>
/// PATRÓN BUILDER: Construcción paso a paso de cursos complejos
/// Permite crear cursos con diferentes configuraciones de manera fluida
/// </summary>
public interface ICourseBuilder
{
    ICourseBuilder SetBasicInfo(string name, string code, string description);
    ICourseBuilder SetCredits(int credits);
    ICourseBuilder SetInstructor(string instructor);
    ICourseBuilder AddTheoryContent(IContent content);
    ICourseBuilder AddPracticeContent(IContent content);
    ICourseBuilder AddExam(IContent exam);
    Course Build();
}

/// <summary>
/// Builder concreto para cursos estándar
/// </summary>
public class StandardCourseBuilder : ICourseBuilder
{
    private readonly Course _course = new();

    public ICourseBuilder SetBasicInfo(string name, string code, string description)
    {
        _course.Name = name;
        _course.Code = code;
        _course.Description = description;
        Console.WriteLine($"[BUILDER] Información básica configurada: {name}");
        return this;
    }

    public ICourseBuilder SetCredits(int credits)
    {
        _course.Credits = credits;
        Console.WriteLine($"[BUILDER] Créditos asignados: {credits}");
        return this;
    }

    public ICourseBuilder SetInstructor(string instructor)
    {
        _course.Instructor = instructor;
        Console.WriteLine($"[BUILDER] Instructor asignado: {instructor}");
        return this;
    }

    public ICourseBuilder AddTheoryContent(IContent content)
    {
        _course.TheoryContent.Add(content);
        Console.WriteLine($"[BUILDER] Contenido teórico agregado: {content.Title}");
        return this;
    }

    public ICourseBuilder AddPracticeContent(IContent content)
    {
        _course.PracticeContent.Add(content);
        Console.WriteLine($"[BUILDER] Contenido práctico agregado: {content.Title}");
        return this;
    }

    public ICourseBuilder AddExam(IContent exam)
    {
        _course.Exams.Add(exam);
        Console.WriteLine($"[BUILDER] Examen agregado: {exam.Title}");
        return this;
    }

    public Course Build()
    {
        Console.WriteLine($"[BUILDER] Curso '{_course.Name}' construido exitosamente");
        
        // Guardar en el repositorio
        CourseManager.Instance.SaveCourse(_course);
        
        return _course;
    }
}

/// <summary>
/// Director que orquesta la construcción de cursos predefinidos
/// </summary>
public class CourseDirector
{
    private readonly ICourseBuilder _builder;

    public CourseDirector(ICourseBuilder builder)
    {
        _builder = builder;
    }

    public Course ConstructBasicCourse(string name, string code, IContentFactory factory)
    {
        Console.WriteLine("[BUILDER] Director construyendo curso básico...");
        return _builder
            .SetBasicInfo(name, code, "Curso básico de introducción")
            .SetCredits(3)
            .AddTheoryContent(factory.CreateVideo())
            .AddTheoryContent(factory.CreateDocument())
            .AddExam(factory.CreateQuiz())
            .Build();
    }

    public Course ConstructAdvancedCourse(string name, string code, IContentFactory factory)
    {
        Console.WriteLine("[BUILDER] Director construyendo curso avanzado...");
        return _builder
            .SetBasicInfo(name, code, "Curso avanzado con contenido completo")
            .SetCredits(5)
            .AddTheoryContent(factory.CreateVideo())
            .AddTheoryContent(factory.CreateDocument())
            .AddPracticeContent(factory.CreateVideo())
            .AddPracticeContent(factory.CreateDocument())
            .AddExam(factory.CreateQuiz())
            .Build();
    }
}

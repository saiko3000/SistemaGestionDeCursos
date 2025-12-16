namespace PlataformaAcademicaModular.CourseBuilder;

/// <summary>
/// Interfaz base para contenido educativo
/// </summary>
public interface IContent
{
    string Title { get; set; }
    string Type { get; }
    int DurationMinutes { get; set; }
    void Display();
}

/// <summary>
/// Contenido tipo Video
/// </summary>
public class VideoContent : IContent
{
    public string Title { get; set; } = string.Empty;
    public string Type => "Video";
    public int DurationMinutes { get; set; }
    public string Quality { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public void Display()
    {
        Console.WriteLine($"    🎥 Video: {Title} ({DurationMinutes} min) - Calidad: {Quality}");
    }
}

/// <summary>
/// Contenido tipo Documento
/// </summary>
public class DocumentContent : IContent
{
    public string Title { get; set; } = string.Empty;
    public string Type => "Documento";
    public int DurationMinutes { get; set; }
    public string Format { get; set; } = string.Empty;
    public int Pages { get; set; }

    public void Display()
    {
        Console.WriteLine($"    📄 Documento: {Title} ({Pages} páginas) - Formato: {Format}");
    }
}

/// <summary>
/// Contenido tipo Quiz/Examen
/// </summary>
public class QuizContent : IContent
{
    public string Title { get; set; } = string.Empty;
    public string Type => "Quiz";
    public int DurationMinutes { get; set; }
    public int QuestionCount { get; set; }
    public string Difficulty { get; set; } = string.Empty;

    public void Display()
    {
        Console.WriteLine($"    ✏️ Quiz: {Title} ({QuestionCount} preguntas) - Dificultad: {Difficulty}");
    }
}

/// <summary>
/// PATRÓN ABSTRACT FACTORY: Crea familias de objetos de contenido relacionados
/// Permite crear contenido básico o avanzado de manera consistente
/// </summary>
public interface IContentFactory
{
    IContent CreateVideo();
    IContent CreateDocument();
    IContent CreateQuiz();
}

/// <summary>
/// Factory para contenido básico
/// </summary>
public class BasicContentFactory : IContentFactory
{
    public IContent CreateVideo()
    {
        Console.WriteLine("[ABSTRACT FACTORY] Creando video básico");
        return new VideoContent
        {
            Title = "Video Introductorio",
            DurationMinutes = 15,
            Quality = "720p",
            Url = "https://example.com/basic-video"
        };
    }

    public IContent CreateDocument()
    {
        Console.WriteLine("[ABSTRACT FACTORY] Creando documento básico");
        return new DocumentContent
        {
            Title = "Guía de Introducción",
            DurationMinutes = 20,
            Format = "PDF",
            Pages = 10
        };
    }

    public IContent CreateQuiz()
    {
        Console.WriteLine("[ABSTRACT FACTORY] Creando quiz básico");
        return new QuizContent
        {
            Title = "Evaluación Básica",
            DurationMinutes = 30,
            QuestionCount = 10,
            Difficulty = "Fácil"
        };
    }
}

/// <summary>
/// Factory para contenido avanzado
/// </summary>
public class AdvancedContentFactory : IContentFactory
{
    public IContent CreateVideo()
    {
        Console.WriteLine("[ABSTRACT FACTORY] Creando video avanzado");
        return new VideoContent
        {
            Title = "Masterclass Avanzada",
            DurationMinutes = 45,
            Quality = "4K",
            Url = "https://example.com/advanced-video"
        };
    }

    public IContent CreateDocument()
    {
        Console.WriteLine("[ABSTRACT FACTORY] Creando documento avanzado");
        return new DocumentContent
        {
            Title = "Manual Técnico Completo",
            DurationMinutes = 60,
            Format = "PDF Interactivo",
            Pages = 50
        };
    }

    public IContent CreateQuiz()
    {
        Console.WriteLine("[ABSTRACT FACTORY] Creando quiz avanzado");
        return new QuizContent
        {
            Title = "Examen Certificación",
            DurationMinutes = 90,
            QuestionCount = 50,
            Difficulty = "Difícil"
        };
    }
}

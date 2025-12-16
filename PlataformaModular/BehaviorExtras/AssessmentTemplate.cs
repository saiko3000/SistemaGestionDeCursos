namespace PlataformaAcademicaModular.BehaviorExtras;

/// <summary>
/// PATRÓN TEMPLATE METHOD: Define el esqueleto del proceso de evaluación
/// Las subclases implementan pasos específicos
/// </summary>
public abstract class AssessmentTemplate
{
    // Template Method - algoritmo general
    public void ConductAssessment(string studentName)
    {
        Console.WriteLine($"\n[TEMPLATE METHOD] Iniciando evaluación para {studentName}");
        
        PrepareAssessment();
        AdministerTest();
        CollectAnswers();
        GradeAssessment();
        ProvideResults();
        
        Console.WriteLine("[TEMPLATE METHOD] Evaluación completada\n");
    }

    // Pasos abstractos - deben ser implementados
    protected abstract void PrepareAssessment();
    protected abstract void AdministerTest();
    protected abstract void GradeAssessment();

    // Pasos con implementación por defecto
    protected virtual void CollectAnswers()
    {
        Console.WriteLine("[TEMPLATE METHOD] Recolectando respuestas...");
    }

    protected virtual void ProvideResults()
    {
        Console.WriteLine("[TEMPLATE METHOD] Proporcionando resultados al estudiante");
    }
}

/// <summary>
/// Evaluación tipo Quiz
/// </summary>
public class QuizAssessment : AssessmentTemplate
{
    protected override void PrepareAssessment()
    {
        Console.WriteLine("[TEMPLATE METHOD] QuizAssessment: Preparando preguntas de opción múltiple");
    }

    protected override void AdministerTest()
    {
        Console.WriteLine("[TEMPLATE METHOD] QuizAssessment: Presentando quiz en línea");
    }

    protected override void GradeAssessment()
    {
        Console.WriteLine("[TEMPLATE METHOD] QuizAssessment: Calificación automática");
    }
}

/// <summary>
/// Evaluación tipo Examen escrito
/// </summary>
public class WrittenExamAssessment : AssessmentTemplate
{
    protected override void PrepareAssessment()
    {
        Console.WriteLine("[TEMPLATE METHOD] WrittenExam: Preparando preguntas abiertas");
    }

    protected override void AdministerTest()
    {
        Console.WriteLine("[TEMPLATE METHOD] WrittenExam: Distribuyendo examen físico");
    }

    protected override void GradeAssessment()
    {
        Console.WriteLine("[TEMPLATE METHOD] WrittenExam: Calificación manual por profesor");
    }

    protected override void ProvideResults()
    {
        base.ProvideResults();
        Console.WriteLine("[TEMPLATE METHOD] WrittenExam: Enviando retroalimentación detallada");
    }
}

/// <summary>
/// Evaluación tipo Proyecto
/// </summary>
public class ProjectAssessment : AssessmentTemplate
{
    protected override void PrepareAssessment()
    {
        Console.WriteLine("[TEMPLATE METHOD] Project: Definiendo requisitos del proyecto");
    }

    protected override void AdministerTest()
    {
        Console.WriteLine("[TEMPLATE METHOD] Project: Asignando proyecto y estableciendo fecha límite");
    }

    protected override void CollectAnswers()
    {
        Console.WriteLine("[TEMPLATE METHOD] Project: Recibiendo entrega del proyecto");
    }

    protected override void GradeAssessment()
    {
        Console.WriteLine("[TEMPLATE METHOD] Project: Evaluando con rúbrica detallada");
    }
}

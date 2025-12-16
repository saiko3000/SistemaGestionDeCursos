namespace PlataformaAcademicaModular.ReportSystem;

/// <summary>
/// Elementos visitables del reporte
/// </summary>
public interface IReportElement
{
    void Accept(IReportVisitor visitor);
}

/// <summary>
/// Elemento: Sección de calificaciones
/// </summary>
public class GradeSection : IReportElement
{
    public string StudentName { get; set; } = string.Empty;
    public Dictionary<string, double> Grades { get; set; } = new();

    public void Accept(IReportVisitor visitor)
    {
        visitor.VisitGradeSection(this);
    }
}

/// <summary>
/// Elemento: Sección de asistencia
/// </summary>
public class AttendanceSection : IReportElement
{
    public string StudentName { get; set; } = string.Empty;
    public int TotalClasses { get; set; }
    public int AttendedClasses { get; set; }

    public void Accept(IReportVisitor visitor)
    {
        visitor.VisitAttendanceSection(this);
    }
}

/// <summary>
/// Elemento: Sección de actividades
/// </summary>
public class ActivitySection : IReportElement
{
    public string StudentName { get; set; } = string.Empty;
    public List<string> CompletedActivities { get; set; } = new();
    public List<string> PendingActivities { get; set; } = new();

    public void Accept(IReportVisitor visitor)
    {
        visitor.VisitActivitySection(this);
    }
}

/// <summary>
/// PATRÓN VISITOR: Permite agregar nuevas operaciones a elementos sin modificarlos
/// Separa algoritmos de la estructura de objetos sobre la que operan
/// </summary>
public interface IReportVisitor
{
    void VisitGradeSection(GradeSection section);
    void VisitAttendanceSection(AttendanceSection section);
    void VisitActivitySection(ActivitySection section);
}

/// <summary>
/// Visitor para calcular estadísticas
/// </summary>
public class StatisticsVisitor : IReportVisitor
{
    public double TotalAverage { get; private set; }
    public double AttendancePercentage { get; private set; }
    public int TotalActivities { get; private set; }

    public void VisitGradeSection(GradeSection section)
    {
        if (section.Grades.Count > 0)
        {
            TotalAverage = section.Grades.Values.Average();
            Console.WriteLine($"[VISITOR] Promedio calculado para {section.StudentName}: {TotalAverage:F2}");
        }
    }

    public void VisitAttendanceSection(AttendanceSection section)
    {
        if (section.TotalClasses > 0)
        {
            AttendancePercentage = (section.AttendedClasses * 100.0) / section.TotalClasses;
            Console.WriteLine($"[VISITOR] Asistencia calculada para {section.StudentName}: {AttendancePercentage:F1}%");
        }
    }

    public void VisitActivitySection(ActivitySection section)
    {
        TotalActivities = section.CompletedActivities.Count + section.PendingActivities.Count;
        var completionRate = section.CompletedActivities.Count * 100.0 / TotalActivities;
        Console.WriteLine($"[VISITOR] Actividades analizadas para {section.StudentName}: {completionRate:F1}% completadas");
    }
}

/// <summary>
/// Visitor para validar datos
/// </summary>
public class ValidationVisitor : IReportVisitor
{
    public List<string> ValidationErrors { get; } = new();

    public void VisitGradeSection(GradeSection section)
    {
        foreach (var grade in section.Grades)
        {
            if (grade.Value < 0 || grade.Value > 100)
            {
                ValidationErrors.Add($"Calificación inválida en {grade.Key}: {grade.Value}");
            }
        }
        Console.WriteLine($"[VISITOR] Validación de calificaciones: {ValidationErrors.Count} errores encontrados");
    }

    public void VisitAttendanceSection(AttendanceSection section)
    {
        if (section.AttendedClasses > section.TotalClasses)
        {
            ValidationErrors.Add("Asistencias mayores que total de clases");
        }
        Console.WriteLine($"[VISITOR] Validación de asistencia completada");
    }

    public void VisitActivitySection(ActivitySection section)
    {
        if (section.CompletedActivities.Count == 0 && section.PendingActivities.Count == 0)
        {
            ValidationErrors.Add("No hay actividades registradas");
        }
        Console.WriteLine($"[VISITOR] Validación de actividades completada");
    }
}

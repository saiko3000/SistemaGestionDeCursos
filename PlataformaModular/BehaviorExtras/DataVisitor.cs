namespace PlataformaAcademicaModular.BehaviorExtras;

/// <summary>
/// Elementos de datos visitables
/// </summary>
public interface IDataElement
{
    void Accept(IDataVisitor visitor);
}

/// <summary>
/// Elemento: Registro de estudiante
/// </summary>
public class StudentRecord : IDataElement
{
    public string Name { get; set; } = string.Empty;
    public List<double> Grades { get; set; } = new();
    public int Absences { get; set; }

    public void Accept(IDataVisitor visitor)
    {
        visitor.VisitStudentRecord(this);
    }
}

/// <summary>
/// Elemento: Registro de curso
/// </summary>
public class CourseRecord : IDataElement
{
    public string CourseName { get; set; } = string.Empty;
    public int EnrolledStudents { get; set; }
    public double AverageGrade { get; set; }

    public void Accept(IDataVisitor visitor)
    {
        visitor.VisitCourseRecord(this);
    }
}

/// <summary>
/// PATRÓN VISITOR: Permite agregar nuevas operaciones a elementos sin modificarlos
/// Separa algoritmos de la estructura de datos
/// </summary>
public interface IDataVisitor
{
    void VisitStudentRecord(StudentRecord record);
    void VisitCourseRecord(CourseRecord record);
}

/// <summary>
/// Visitor: Calculador de estadísticas
/// </summary>
public class StatisticsCalculatorVisitor : IDataVisitor
{
    public double TotalAverage { get; private set; }
    public int TotalStudents { get; private set; }

    public void VisitStudentRecord(StudentRecord record)
    {
        if (record.Grades.Count > 0)
        {
            var avg = record.Grades.Average();
            TotalAverage += avg;
            TotalStudents++;
            Console.WriteLine($"[VISITOR] Estadísticas de {record.Name}: Promedio {avg:F2}, Ausencias {record.Absences}");
        }
    }

    public void VisitCourseRecord(CourseRecord record)
    {
        Console.WriteLine($"[VISITOR] Estadísticas del curso {record.CourseName}:");
        Console.WriteLine($"  Estudiantes inscritos: {record.EnrolledStudents}");
        Console.WriteLine($"  Promedio del curso: {record.AverageGrade:F2}");
    }

    public double GetOverallAverage()
    {
        return TotalStudents > 0 ? TotalAverage / TotalStudents : 0;
    }
}

/// <summary>
/// Visitor: Validador de datos
/// </summary>
public class DataValidatorVisitor : IDataVisitor
{
    public List<string> Errors { get; } = new();

    public void VisitStudentRecord(StudentRecord record)
    {
        Console.WriteLine($"[VISITOR] Validando registro de {record.Name}");

        if (string.IsNullOrWhiteSpace(record.Name))
        {
            Errors.Add("Nombre de estudiante vacío");
        }

        if (record.Grades.Any(g => g < 0 || g > 100))
        {
            Errors.Add($"Calificaciones inválidas para {record.Name}");
        }

        if (record.Absences < 0)
        {
            Errors.Add($"Ausencias negativas para {record.Name}");
        }
    }

    public void VisitCourseRecord(CourseRecord record)
    {
        Console.WriteLine($"[VISITOR] Validando registro del curso {record.CourseName}");

        if (string.IsNullOrWhiteSpace(record.CourseName))
        {
            Errors.Add("Nombre de curso vacío");
        }

        if (record.EnrolledStudents < 0)
        {
            Errors.Add($"Número de estudiantes negativo en {record.CourseName}");
        }

        if (record.AverageGrade < 0 || record.AverageGrade > 100)
        {
            Errors.Add($"Promedio inválido en {record.CourseName}");
        }
    }

    public bool IsValid()
    {
        return Errors.Count == 0;
    }
}

/// <summary>
/// Visitor: Generador de reportes
/// </summary>
public class ReportGeneratorVisitor : IDataVisitor
{
    private readonly System.Text.StringBuilder _report = new();

    public void VisitStudentRecord(StudentRecord record)
    {
        _report.AppendLine($"📊 Estudiante: {record.Name}");
        _report.AppendLine($"   Calificaciones: {string.Join(", ", record.Grades.Select(g => g.ToString("F1")))}");
        _report.AppendLine($"   Promedio: {(record.Grades.Count > 0 ? record.Grades.Average() : 0):F2}");
        _report.AppendLine($"   Ausencias: {record.Absences}");
        Console.WriteLine($"[VISITOR] Reporte generado para {record.Name}");
    }

    public void VisitCourseRecord(CourseRecord record)
    {
        _report.AppendLine($"📚 Curso: {record.CourseName}");
        _report.AppendLine($"   Estudiantes: {record.EnrolledStudents}");
        _report.AppendLine($"   Promedio: {record.AverageGrade:F2}");
        Console.WriteLine($"[VISITOR] Reporte generado para {record.CourseName}");
    }

    public string GetReport()
    {
        return _report.ToString();
    }
}

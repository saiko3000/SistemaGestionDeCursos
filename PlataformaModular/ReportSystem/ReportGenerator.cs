namespace PlataformaAcademicaModular.ReportSystem;

/// <summary>
/// Datos del reporte
/// </summary>
public class ReportData
{
    public string Title { get; set; } = string.Empty;
    public DateTime GeneratedDate { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();
}

/// <summary>
/// PATRÓN TEMPLATE METHOD: Define el esqueleto del algoritmo de generación de reportes
/// Las subclases implementan pasos específicos sin cambiar la estructura general
/// </summary>
public abstract class ReportGenerator
{
    // Template Method - define el algoritmo general
    public string GenerateReport(ReportData data)
    {
        Console.WriteLine("[TEMPLATE METHOD] Iniciando generación de reporte...");
        
        var report = new System.Text.StringBuilder();
        
        AddHeader(report, data);
        AddContent(report, data);
        AddFooter(report, data);
        ApplyFormatting(report);
        
        Console.WriteLine("[TEMPLATE METHOD] Reporte generado exitosamente");
        return report.ToString();
    }

    // Pasos del algoritmo - algunos abstractos, otros con implementación por defecto
    protected abstract void AddHeader(System.Text.StringBuilder report, ReportData data);
    protected abstract void AddContent(System.Text.StringBuilder report, ReportData data);
    
    protected virtual void AddFooter(System.Text.StringBuilder report, ReportData data)
    {
        report.AppendLine($"\n--- Generado el {data.GeneratedDate:dd/MM/yyyy HH:mm} ---");
    }
    
    protected virtual void ApplyFormatting(System.Text.StringBuilder report)
    {
        // Formateo por defecto
    }
}

/// <summary>
/// Generador de reportes académicos
/// </summary>
public class AcademicReportGenerator : ReportGenerator
{
    protected override void AddHeader(System.Text.StringBuilder report, ReportData data)
    {
        report.AppendLine("╔════════════════════════════════════════╗");
        report.AppendLine($"║  {data.Title.PadRight(36)}  ║");
        report.AppendLine("╔════════════════════════════════════════╗");
        Console.WriteLine("[TEMPLATE METHOD] Encabezado académico agregado");
    }

    protected override void AddContent(System.Text.StringBuilder report, ReportData data)
    {
        report.AppendLine("\n📊 DATOS ACADÉMICOS:");
        foreach (var item in data.Data)
        {
            report.AppendLine($"  • {item.Key}: {item.Value}");
        }
        Console.WriteLine("[TEMPLATE METHOD] Contenido académico agregado");
    }
}

/// <summary>
/// Generador de reportes de asistencia
/// </summary>
public class AttendanceReportGenerator : ReportGenerator
{
    protected override void AddHeader(System.Text.StringBuilder report, ReportData data)
    {
        report.AppendLine("═══════════════════════════════════════");
        report.AppendLine($"  REPORTE DE ASISTENCIA");
        report.AppendLine($"  {data.Title}");
        report.AppendLine("═══════════════════════════════════════");
        Console.WriteLine("[TEMPLATE METHOD] Encabezado de asistencia agregado");
    }

    protected override void AddContent(System.Text.StringBuilder report, ReportData data)
    {
        report.AppendLine("\n📋 REGISTRO DE ASISTENCIA:");
        foreach (var item in data.Data)
        {
            report.AppendLine($"  ✓ {item.Key}: {item.Value}");
        }
        Console.WriteLine("[TEMPLATE METHOD] Contenido de asistencia agregado");
    }
}

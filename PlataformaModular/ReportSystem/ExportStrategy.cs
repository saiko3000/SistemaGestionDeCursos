namespace PlataformaAcademicaModular.ReportSystem;

/// <summary>
/// PATRÓN STRATEGY: Define una familia de algoritmos de exportación intercambiables
/// Permite cambiar el formato de exportación en tiempo de ejecución
/// </summary>
public interface IExportStrategy
{
    string Export(string reportContent, string fileName);
}

/// <summary>
/// Estrategia de exportación a PDF
/// </summary>
public class PdfExportStrategy : IExportStrategy
{
    public string Export(string reportContent, string fileName)
    {
        Console.WriteLine($"[STRATEGY] Exportando a PDF: {fileName}.pdf");
        
        // Simulación de exportación PDF
        var pdfContent = $"[PDF FORMAT]\n{reportContent}\n[END PDF]";
        
        Console.WriteLine($"[STRATEGY] PDF generado exitosamente ({reportContent.Length} caracteres)");
        return pdfContent;
    }
}

/// <summary>
/// Estrategia de exportación a Excel
/// </summary>
public class ExcelExportStrategy : IExportStrategy
{
    public string Export(string reportContent, string fileName)
    {
        Console.WriteLine($"[STRATEGY] Exportando a Excel: {fileName}.xlsx");
        
        // Simulación de exportación Excel
        var excelContent = $"[EXCEL FORMAT]\nWorksheet: Report\n{reportContent}\n[END EXCEL]";
        
        Console.WriteLine($"[STRATEGY] Excel generado exitosamente");
        return excelContent;
    }
}

/// <summary>
/// Estrategia de exportación a JSON
/// </summary>
public class JsonExportStrategy : IExportStrategy
{
    public string Export(string reportContent, string fileName)
    {
        Console.WriteLine($"[STRATEGY] Exportando a JSON: {fileName}.json");
        
        // Simulación de exportación JSON
        var jsonContent = $@"{{
    ""fileName"": ""{fileName}"",
    ""generatedAt"": ""{DateTime.Now:yyyy-MM-dd HH:mm:ss}"",
    ""content"": ""{reportContent.Replace("\n", "\\n").Replace("\"", "\\\"")}""
}}";
        
        Console.WriteLine($"[STRATEGY] JSON generado exitosamente");
        return jsonContent;
    }
}

/// <summary>
/// Contexto que utiliza las estrategias de exportación
/// </summary>
public class ReportExporter
{
    private IExportStrategy _strategy;

    public ReportExporter(IExportStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IExportStrategy strategy)
    {
        _strategy = strategy;
        Console.WriteLine($"[STRATEGY] Estrategia de exportación cambiada a {strategy.GetType().Name}");
    }

    public string ExportReport(string reportContent, string fileName)
    {
        return _strategy.Export(reportContent, fileName);
    }
}

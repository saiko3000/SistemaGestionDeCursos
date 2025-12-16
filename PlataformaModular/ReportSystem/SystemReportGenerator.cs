namespace PlataformaAcademicaModular.ReportSystem;

/// <summary>
/// Generador de reportes del sistema completo
/// Lee datos reales de los repositorios
/// </summary>
public class SystemReportGenerator : ReportGenerator
{
    protected override void AddHeader(System.Text.StringBuilder report, ReportData data)
    {
        report.AppendLine("╔════════════════════════════════════════════════════════════════╗");
        report.AppendLine("║           REPORTE DEL SISTEMA ACADÉMICO                        ║");
        report.AppendLine("╠════════════════════════════════════════════════════════════════╣");
        report.AppendLine($"║  Generado: {data.GeneratedDate:dd/MM/yyyy HH:mm:ss}                          ║");
        report.AppendLine("╚════════════════════════════════════════════════════════════════╝");
    }

    protected override void AddContent(System.Text.StringBuilder report, ReportData data)
    {
        report.AppendLine("\n📊 ESTADÍSTICAS DEL SISTEMA:\n");
        
        foreach (var item in data.Data)
        {
            report.AppendLine($"  ▪ {item.Key}: {item.Value}");
        }
    }

    protected override void AddFooter(System.Text.StringBuilder report, ReportData data)
    {
        report.AppendLine($"\n{new string('─', 64)}");
        report.AppendLine($"Sistema de Gestión Académica Modular v1.0");
        report.AppendLine($"Reporte generado el {data.GeneratedDate:dd/MM/yyyy} a las {data.GeneratedDate:HH:mm:ss}");
    }
}

/// <summary>
/// Servicio para generar reportes del sistema
/// </summary>
public static class ReportService
{
    /// <summary>
    /// Genera un reporte completo del sistema con datos reales
    /// </summary>
    public static void GenerateSystemReport()
    {
        Console.WriteLine("\n[TEMPLATE METHOD] Generando reporte del sistema...\n");

        var reportData = new ReportData
        {
            Title = "Reporte del Sistema",
            GeneratedDate = DateTime.Now,
            Data = new Dictionary<string, object>
            {
                { "Total de Usuarios Registrados", UserManagement.UserSession.GetUserCount() },
                { "Total de Cursos Disponibles", CourseBuilder.CourseManager.Instance.GetCourseCount() },
                { "Total de Notificaciones Enviadas", NotificationCenter.NotificationService.Instance.GetNotificationCount() },
                { "Usuario Actual", UserManagement.UserSession.Instance.CurrentUser?.Name ?? "Ninguno" },
                { "Estado de Sesión", UserManagement.UserSession.Instance.IsLoggedIn ? "Activa" : "Inactiva" }
            }
        };

        var generator = new SystemReportGenerator();
        var report = generator.GenerateReport(reportData);
        
        Console.WriteLine(report);

        // Mostrar detalles adicionales
        Console.WriteLine("\n📚 DETALLE DE CURSOS:");
        CourseBuilder.CourseManager.Instance.DisplayAllCourses();

        Console.WriteLine("\n👥 USUARIOS REGISTRADOS:");
        var users = UserManagement.UserSession.GetAllUsers();
        if (users.Count == 0)
        {
            Console.WriteLine("  No hay usuarios registrados");
        }
        else
        {
            foreach (var user in users)
            {
                user.DisplayInfo();
            }
        }
    }
}

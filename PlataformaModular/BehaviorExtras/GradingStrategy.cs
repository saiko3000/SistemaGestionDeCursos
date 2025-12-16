namespace PlataformaAcademicaModular.BehaviorExtras;

/// <summary>
/// PATRÓN STRATEGY: Define una familia de algoritmos de calificación
/// Los hace intercambiables y permite que varíen independientemente
/// </summary>
public interface IGradingStrategy
{
    string CalculateGrade(double score);
    string GetStrategyName();
}

/// <summary>
/// Estrategia: Calificación por letras (A-F)
/// </summary>
public class LetterGradingStrategy : IGradingStrategy
{
    public string GetStrategyName() => "Calificación por Letras";

    public string CalculateGrade(double score)
    {
        Console.WriteLine($"[STRATEGY] Calculando calificación por letras para {score}");
        
        return score switch
        {
            >= 90 => "A (Excelente)",
            >= 80 => "B (Muy Bueno)",
            >= 70 => "C (Bueno)",
            >= 60 => "D (Suficiente)",
            _ => "F (Reprobado)"
        };
    }
}

/// <summary>
/// Estrategia: Calificación por porcentaje
/// </summary>
public class PercentageGradingStrategy : IGradingStrategy
{
    public string GetStrategyName() => "Calificación por Porcentaje";

    public string CalculateGrade(double score)
    {
        Console.WriteLine($"[STRATEGY] Calculando calificación por porcentaje para {score}");
        return $"{score}% - {GetPerformanceLevel(score)}";
    }

    private string GetPerformanceLevel(double score)
    {
        return score switch
        {
            >= 85 => "Sobresaliente",
            >= 70 => "Notable",
            >= 60 => "Aprobado",
            _ => "Reprobado"
        };
    }
}

/// <summary>
/// Estrategia: Calificación Aprobado/Reprobado
/// </summary>
public class PassFailGradingStrategy : IGradingStrategy
{
    private readonly double _passingScore;

    public PassFailGradingStrategy(double passingScore = 60)
    {
        _passingScore = passingScore;
    }

    public string GetStrategyName() => "Calificación Aprobado/Reprobado";

    public string CalculateGrade(double score)
    {
        Console.WriteLine($"[STRATEGY] Calculando aprobado/reprobado para {score}");
        return score >= _passingScore ? "✅ APROBADO" : "❌ REPROBADO";
    }
}

/// <summary>
/// Contexto que utiliza las estrategias de calificación
/// </summary>
public class GradeCalculator
{
    private IGradingStrategy _strategy;

    public GradeCalculator(IGradingStrategy strategy)
    {
        _strategy = strategy;
        Console.WriteLine($"[STRATEGY] Calculadora configurada con: {strategy.GetStrategyName()}");
    }

    public void SetStrategy(IGradingStrategy strategy)
    {
        _strategy = strategy;
        Console.WriteLine($"[STRATEGY] Estrategia cambiada a: {strategy.GetStrategyName()}");
    }

    public string Grade(double score)
    {
        return _strategy.CalculateGrade(score);
    }
}

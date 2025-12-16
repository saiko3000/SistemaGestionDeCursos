namespace PlataformaAcademicaModular.BehaviorExtras;

/// <summary>
/// PATRÓN INTERPRETER: Interpreta y evalúa expresiones en un lenguaje específico
/// Define una gramática y un intérprete para evaluarla
/// </summary>
public interface IExpression
{
    bool Interpret(Context context);
}

/// <summary>
/// Contexto que contiene información para la interpretación
/// </summary>
public class Context
{
    public Dictionary<string, bool> Variables { get; } = new();

    public void SetVariable(string name, bool value)
    {
        Variables[name] = value;
        Console.WriteLine($"[INTERPRETER] Variable '{name}' = {value}");
    }

    public bool GetVariable(string name)
    {
        return Variables.TryGetValue(name, out bool value) && value;
    }
}

/// <summary>
/// Expresión terminal: Variable
/// </summary>
public class VariableExpression : IExpression
{
    private readonly string _variableName;

    public VariableExpression(string variableName)
    {
        _variableName = variableName;
    }

    public bool Interpret(Context context)
    {
        bool result = context.GetVariable(_variableName);
        Console.WriteLine($"[INTERPRETER] Evaluando variable '{_variableName}' = {result}");
        return result;
    }
}

/// <summary>
/// Expresión no terminal: AND lógico
/// </summary>
public class AndExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    public AndExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public bool Interpret(Context context)
    {
        bool result = _left.Interpret(context) && _right.Interpret(context);
        Console.WriteLine($"[INTERPRETER] AND = {result}");
        return result;
    }
}

/// <summary>
/// Expresión no terminal: OR lógico
/// </summary>
public class OrExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    public OrExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public bool Interpret(Context context)
    {
        bool result = _left.Interpret(context) || _right.Interpret(context);
        Console.WriteLine($"[INTERPRETER] OR = {result}");
        return result;
    }
}

/// <summary>
/// Expresión no terminal: NOT lógico
/// </summary>
public class NotExpression : IExpression
{
    private readonly IExpression _expression;

    public NotExpression(IExpression expression)
    {
        _expression = expression;
    }

    public bool Interpret(Context context)
    {
        bool result = !_expression.Interpret(context);
        Console.WriteLine($"[INTERPRETER] NOT = {result}");
        return result;
    }
}

/// <summary>
/// Intérprete de reglas de acceso académico
/// Ejemplo: "isStudent AND (hasEnrolled OR isAdmin)"
/// </summary>
public class AccessRuleInterpreter
{
    public static bool EvaluateAccessRule(string rule, Context context)
    {
        Console.WriteLine($"\n[INTERPRETER] Evaluando regla de acceso: {rule}");
        
        // Construir expresión basada en la regla
        IExpression expression = ParseRule(rule);
        
        bool result = expression.Interpret(context);
        Console.WriteLine($"[INTERPRETER] Resultado final: {(result ? "✅ ACCESO PERMITIDO" : "❌ ACCESO DENEGADO")}\n");
        
        return result;
    }

    private static IExpression ParseRule(string rule)
    {
        // Interpretación simplificada de reglas comunes
        return rule.ToLower() switch
        {
            "student_access" => new AndExpression(
                new VariableExpression("isStudent"),
                new VariableExpression("hasEnrolled")
            ),
            "admin_or_teacher" => new OrExpression(
                new VariableExpression("isAdmin"),
                new VariableExpression("isTeacher")
            ),
            "full_access" => new OrExpression(
                new VariableExpression("isAdmin"),
                new AndExpression(
                    new VariableExpression("isTeacher"),
                    new VariableExpression("hasPermission")
                )
            ),
            _ => new VariableExpression(rule)
        };
    }
}

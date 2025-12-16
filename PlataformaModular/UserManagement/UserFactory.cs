namespace PlataformaAcademicaModular.UserManagement;

/// <summary>
/// PATRÓN FACTORY METHOD: Crea diferentes tipos de usuarios sin exponer la lógica de creación
/// Permite agregar nuevos tipos de usuarios sin modificar el código cliente
/// </summary>
public abstract class UserFactory
{
    public abstract IUser CreateUser(string name, string email, string additionalInfo);

    public IUser CreateAndRegister(string name, string email, string additionalInfo)
    {
        var user = CreateUser(name, email, additionalInfo);
        Console.WriteLine($"[FACTORY METHOD] Usuario creado: {user.Role}");
        return user;
    }
}

/// <summary>
/// Factory concreta para crear Estudiantes
/// </summary>
public class StudentFactory : UserFactory
{
    public override IUser CreateUser(string name, string email, string studentId)
    {
        return new Student(name, email, studentId);
    }
}

/// <summary>
/// Factory concreta para crear Profesores
/// </summary>
public class TeacherFactory : UserFactory
{
    public override IUser CreateUser(string name, string email, string department)
    {
        return new Teacher(name, email, department);
    }
}

/// <summary>
/// Factory concreta para crear Administradores
/// </summary>
public class AdministratorFactory : UserFactory
{
    public override IUser CreateUser(string name, string email, string adminLevel)
    {
        return new Administrator(name, email, adminLevel);
    }
}

/// <summary>
/// Factory Manager que selecciona la factory apropiada
/// </summary>
public static class UserFactoryManager
{
    public static IUser CreateUser(string userType, string name, string email, string additionalInfo)
    {
        UserFactory factory = userType.ToLower() switch
        {
            "estudiante" or "student" => new StudentFactory(),
            "profesor" or "teacher" => new TeacherFactory(),
            "administrador" or "admin" => new AdministratorFactory(),
            _ => throw new ArgumentException($"Tipo de usuario desconocido: {userType}")
        };

        return factory.CreateAndRegister(name, email, additionalInfo);
    }
}

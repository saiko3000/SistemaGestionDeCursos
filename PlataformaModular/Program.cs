using PlataformaAcademicaModular.UserManagement;
using PlataformaAcademicaModular.CourseBuilder;
using PlataformaAcademicaModular.ReportSystem;
using PlataformaAcademicaModular.NotificationCenter;
using PlataformaAcademicaModular.AccessControl;
using PlataformaAcademicaModular.UIAdapter;
using PlataformaAcademicaModular.ResourceOptimizer;
using PlataformaAcademicaModular.BehaviorExtras;

namespace PlataformaAcademicaModular;

/// <summary>
/// SISTEMA DE GESTIÓN ACADÉMICA MODULAR - PROTOTIPO FUNCIONAL
/// Demostración completa de 23 Patrones de Diseño GoF
/// Con persistencia en memoria y menú interactivo
/// 
/// Autor: Arquitecto de Software Senior
/// Framework: .NET 8.0
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        MostrarBanner();
        InicializarSistema();
        MenuPrincipal();
    }

    static void MostrarBanner()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║     SISTEMA DE GESTIÓN ACADÉMICA MODULAR - C# .NET 8.0         ║");
        Console.WriteLine("║          Demostración de 23 Patrones de Diseño                 ║");
        Console.WriteLine("║                  PROTOTIPO FUNCIONAL v1.0                      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    static void InicializarSistema()
    {
        Console.WriteLine("⚙️  Inicializando sistema...\n");
        
        // Inicializar singletons (esto carga usuarios por defecto)
        var session = UserSession.Instance;
        var notificationService = NotificationService.Instance;
        var courseManager = CourseManager.Instance;
        
        Console.WriteLine("✅ Sistema inicializado correctamente\n");
        Console.WriteLine("💡 Usuarios de demostración disponibles:");
        Console.WriteLine("   - admin / admin123 (Administrador)");
        Console.WriteLine("   - profesor1 / prof123 (Profesor)");
        Console.WriteLine("   - estudiante1 / est123 (Estudiante)\n");
        
        PausarConsola();
    }

    static bool FlujoProfesor()
    {
        while (true)
        {
            Console.Clear();
            MostrarEncabezado();
            
            var session = UserSession.Instance;
            var courses = CourseManager.Instance.GetAllCourses()
                            .Where(c => c.Instructor == session.CurrentUser!.Name)
                            .ToList();

            Console.WriteLine("📚 MIS CURSOS ASIGNADOS:");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");

            if (courses.Count == 0)
            {
                Console.WriteLine("\n❌ No tienes cursos asignados actualmente.");
            }
            else
            {
                for (int i = 0; i < courses.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}️⃣  {courses[i].Name} ({courses[i].Code})");
                }
            }

            Console.WriteLine($"  {courses.Count + 1}️⃣  Cerrar Sesión");
            Console.WriteLine("  0️⃣  Salir");
            
            Console.Write("\n👉 Seleccione una opción: ");
            string? input = Console.ReadLine();

            if (input == "0") return true; // Salir de la app
            
            if (int.TryParse(input, out int opcion))
            {
                if (opcion == courses.Count + 1)
                {
                    session.Logout();
                    return false; // Volver al menú principal (login)
                }
                
                if (opcion > 0 && opcion <= courses.Count)
                {
                    MenuAccionesCurso(courses[opcion - 1]);
                    continue;
                }
            }
            
            Console.WriteLine("\n❌ Opción no válida.");
            PausarConsola();
        }
    }

    static void MenuAccionesCurso(Course course)
    {
        bool regresar = false;
        do
        {
            Console.Clear();
            MostrarEncabezado();
            Console.WriteLine($"📘 CURSO ACTUAL: {course.Name} ({course.Code})");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            
            Console.WriteLine("📋 MENÚ DEL CURSO:");
            Console.WriteLine();
            Console.WriteLine("  1️⃣  Gestionar Contenido del Curso");
            Console.WriteLine("  2️⃣  Crear Actividades");
            Console.WriteLine("  3️⃣  Evaluar Estudiantes");
            Console.WriteLine("  4️⃣  Control de Asistencia");
            Console.WriteLine("  5️⃣  Comunicación");
            Console.WriteLine("  6️⃣  Monitoreo del Avance");
            Console.WriteLine("  7️⃣  Volver a Mis Cursos");
            Console.WriteLine("  0️⃣  Salir");
            
            Console.Write("\n👉 Seleccione una opción: ");
            string? opcion = Console.ReadLine();
            
            switch (opcion)
            {
                case "1": GestionarContenidoCurso(course); break;
                case "2": CrearActividades(course); break;
                case "3": EvaluarEstudiantes(course); break;
                case "4": ControlAsistencia(course); break;
                case "5": ComunicacionProfesor(course); break;
                case "6": MonitoreoAvance(course); break;
                case "7": regresar = true; break;
                case "0": 
                    if (ConfirmarSalida()) Environment.Exit(0); 
                    break;
                default:
                    Console.WriteLine("\n❌ Opción no válida.");
                    PausarConsola();
                    break;
            }
            
        } while (!regresar);
    }

    static void MenuPrincipal()
    {
        bool salir = false;

        do
        {
            Console.Clear();
            MostrarEncabezado();
            MostrarMenuOpciones();

            Console.Write("\n👉 Seleccione una opción: ");
            string? opcion = Console.ReadLine();

            Console.WriteLine();

            var session = UserSession.Instance;
            string role = session.IsLoggedIn ? session.CurrentUser!.Role : "Guest";

            switch (opcion)
            {
                case "1":
                    if (role == "Guest") RegistrarUsuario();
                    else if (role == "Estudiante") MisCursos();
                    else if (role == "Profesor") MostrarAccesoDenegado("Los profesores deben seleccionar un curso primero.");
                    else if (role == "Administrador") GestionarUsuarios();
                    break;
                case "2":
                    if (role == "Guest") IniciarSesion();
                    else if (role == "Estudiante") DescargarMateriales();
                    else if (role == "Profesor") MostrarAccesoDenegado("Los profesores deben seleccionar un curso primero.");
                    else if (role == "Administrador") GestionarCursos();
                    break;
                case "3":
                    if (role == "Estudiante") EntregarActividades();
                    else if (role == "Profesor") MostrarAccesoDenegado("Los profesores deben seleccionar un curso primero.");
                    else if (role == "Administrador") GestionAcademica();
                    else MostrarAccesoDenegado("Opción no disponible para su rol.");
                    break;
                case "4":
                    if (role == "Estudiante") ConsultarCalificaciones();
                    else if (role == "Profesor") MostrarAccesoDenegado("Los profesores deben seleccionar un curso primero.");
                    else if (role == "Administrador") VerReportesEstadisticas();
                    else MostrarAccesoDenegado("Opción no disponible para su rol.");
                    break;
                case "5":
                    if (role == "Estudiante") Mensajes();
                    else if (role == "Profesor") MostrarAccesoDenegado("Los profesores deben seleccionar un curso primero.");
                    else if (role == "Administrador") ConfiguracionSistema();
                    else MostrarAccesoDenegado("Opción no disponible para su rol.");
                    break;
                case "6":
                    if (role == "Estudiante") Tramites();
                    else if (role == "Profesor") MostrarAccesoDenegado("Los profesores deben seleccionar un curso primero.");
                    else if (role == "Administrador") CerrarSesion();
                    else MostrarAccesoDenegado("Opción no disponible para su rol.");
                    break;
                case "7":
                    if (role != "Guest" && role != "Administrador") CerrarSesion();
                    else MostrarAccesoDenegado("Opción no disponible.");
                    break;
                case "0":
                    salir = ConfirmarSalida();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Opción no válida. Intente nuevamente.");
                    Console.ResetColor();
                    PausarConsola();
                    break;
            }

        } while (!salir);

        Console.WriteLine("\n👋 ¡Hasta pronto!\n");
    }

    static void MostrarMenuOpciones()
    {
        var session = UserSession.Instance;
        string role = session.IsLoggedIn ? session.CurrentUser!.Role : "Guest";

        Console.WriteLine("📋 MENÚ PRINCIPAL:");
        Console.WriteLine();

        if (role == "Guest")
        {
            Console.WriteLine("  1️⃣  Registrar Usuario");
            Console.WriteLine("  2️⃣  Iniciar Sesión");
            Console.WriteLine("  0️⃣  Salir");
        }
        else if (role == "Estudiante")
        {
            Console.WriteLine("  1️⃣  Mis Cursos");
            Console.WriteLine("  2️⃣  Descargar Materiales");
            Console.WriteLine("  3️⃣  Entregar Actividades");
            Console.WriteLine("  4️⃣  Consultar Calificaciones");
            Console.WriteLine("  5️⃣  Mensajes");
            Console.WriteLine("  6️⃣  Trámites");
            Console.WriteLine("  7️⃣  Cerrar Sesión");
            Console.WriteLine("  0️⃣  Salir");
        }
        else if (role == "Profesor")
        {
            Console.WriteLine("  1️⃣  Gestionar Contenido del Curso");
            Console.WriteLine("  2️⃣  Crear Actividades");
            Console.WriteLine("  3️⃣  Evaluar Estudiantes");
            Console.WriteLine("  4️⃣  Control de Asistencia");
            Console.WriteLine("  5️⃣  Comunicación");
            Console.WriteLine("  6️⃣  Monitoreo del Avance");
            Console.WriteLine("  7️⃣  Cerrar Sesión");
            Console.WriteLine("  0️⃣  Salir");
        }
        else if (role == "Administrador")
        {
            Console.WriteLine("  1️⃣  Gestión de Usuarios");
            Console.WriteLine("  2️⃣  Gestión de Cursos");
            Console.WriteLine("  3️⃣  Gestión Académica");
            Console.WriteLine("  4️⃣  Ver Reportes y Estadísticas");
            Console.WriteLine("  5️⃣  Configuración del Sistema");
            Console.WriteLine("  6️⃣  Cerrar Sesión");
            Console.WriteLine("  0️⃣  Salir");
        }
    }

    static void MostrarAccesoDenegado(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n⛔ ACCESO DENEGADO");
        Console.WriteLine($"   {mensaje}");
        Console.ResetColor();
        PausarConsola();
    }

    static void RegistrarUsuario()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   REGISTRO DE NUEVO USUARIO");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();

        Console.Write("Nombre de usuario: ");
        string? username = Console.ReadLine();

        Console.Write("Contraseña: ");
        string? password = LeerPasswordOculto();

        Console.WriteLine("\nSeleccione el rol:");
        Console.WriteLine("  1. Estudiante");
        Console.WriteLine("  2. Profesor");
        Console.WriteLine("  3. Administrador");
        Console.Write("Opción: ");
        string? rolOpcion = Console.ReadLine();

        string role = rolOpcion switch
        {
            "1" => "estudiante",
            "2" => "profesor",
            "3" => "administrador",
            _ => "estudiante"
        };

        Console.Write($"Información adicional ({(role == "estudiante" ? "ID Estudiante" : role == "profesor" ? "Departamento" : "Nivel Admin")}): ");
        string? additionalInfo = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n❌ Usuario y contraseña son obligatorios");
            Console.ResetColor();
        }
        else
        {
            var session = UserSession.Instance;
            session.Register(username, password, role, additionalInfo ?? "N/A");
        }

        PausarConsola();
    }

    static void IniciarSesion()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   INICIO DE SESIÓN");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();

        var session = UserSession.Instance;

        if (session.IsLoggedIn)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  Ya hay una sesión activa para {session.CurrentUser!.Name}");
            Console.WriteLine("   Debe cerrar sesión primero.");
            Console.ResetColor();
            PausarConsola();
            return;
        }

        Console.Write("Usuario: ");
        string? username = Console.ReadLine();

        Console.Write("Contraseña: ");
        string? password = LeerPasswordOculto();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n❌ Usuario y contraseña son obligatorios");
            Console.ResetColor();
        }
        else
        {
            bool loginExitoso = session.Login(username, password);

            if (loginExitoso)
            {
                // Disparar notificación automática
                NotificationService.Instance.NotifyUserLogin(username, session.CurrentUser!.Role);
                
                // Si es profesor, redirigir al flujo de cursos
                if (session.CurrentUser!.Role == "Profesor")
                {
                    PausarConsola();
                    bool salir = FlujoProfesor();
                    if (salir)
                    {
                        Environment.Exit(0);
                    }
                    return;
                }
            }
        }

        PausarConsola();
    }

    static void CrearCurso()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   CREAR NUEVO CURSO (BUILDER PATTERN)");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();

        var session = UserSession.Instance;

        if (!session.IsLoggedIn)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Debe iniciar sesión para crear cursos");
            Console.ResetColor();
            PausarConsola();
            return;
        }

        if (session.CurrentUser!.Role != "Profesor" && session.CurrentUser.Role != "Administrador")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Solo profesores y administradores pueden crear cursos");
            Console.ResetColor();
            PausarConsola();
            return;
        }

        Console.Write("Nombre del curso: ");
        string? nombre = Console.ReadLine();

        Console.Write("Código del curso (dejar vacío para auto-generar): ");
        string? codigo = Console.ReadLine();

        Console.Write("Descripción: ");
        string? descripcion = Console.ReadLine();

        Console.Write("Créditos: ");
        string? creditosStr = Console.ReadLine();
        int creditos = int.TryParse(creditosStr, out int c) ? c : 3;

        Console.WriteLine("\nSeleccione tipo de contenido:");
        Console.WriteLine("  1. Básico");
        Console.WriteLine("  2. Avanzado");
        Console.Write("Opción: ");
        string? tipoContenido = Console.ReadLine();

        IContentFactory factory = tipoContenido == "2" 
            ? new AdvancedContentFactory() 
            : new BasicContentFactory();

        Console.WriteLine("\n[BUILDER] Construyendo curso...\n");

        var builder = new StandardCourseBuilder();
        var curso = builder
            .SetBasicInfo(nombre ?? "Curso Sin Nombre", codigo ?? "", descripcion ?? "Sin descripción")
            .SetCredits(creditos)
            .SetInstructor(session.CurrentUser.Name)
            .AddTheoryContent(factory.CreateVideo())
            .AddTheoryContent(factory.CreateDocument())
            .AddPracticeContent(factory.CreateVideo())
            .AddExam(factory.CreateQuiz())
            .Build();

        Console.WriteLine();
        curso.DisplayCourse();

        // Disparar notificación automática
        NotificationService.Instance.NotifyCourseCreated(curso.Name, curso.Code);

        PausarConsola();
    }

    static void VerNotificaciones()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   CENTRO DE NOTIFICACIONES");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();

        NotificationService.Instance.DisplayNotificationHistory();

        PausarConsola();
    }

    static void GenerarReporte()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   GENERADOR DE REPORTES");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();

        var session = UserSession.Instance;

        // Restricción de privilegios: Estudiantes no pueden generar reportes
        if (session.IsLoggedIn && session.CurrentUser!.Role == "Estudiante")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("⛔ ACCESO DENEGADO");
            Console.WriteLine("   Los estudiantes no tienen permisos para generar reportes del sistema.");
            Console.ResetColor();
            PausarConsola();
            return;
        }

        ReportService.GenerateSystemReport();

        PausarConsola();
    }

    static void InscribirseEnCurso()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   INSCRIPCIÓN A CURSOS");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();

        var session = UserSession.Instance;

        if (!session.IsLoggedIn)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Debe iniciar sesión para inscribirse");
            Console.ResetColor();
            PausarConsola();
            return;
        }

        if (session.CurrentUser!.Role != "Estudiante")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Solo los estudiantes pueden inscribirse en cursos");
            Console.ResetColor();
            PausarConsola();
            return;
        }

        // Mostrar cursos disponibles primero
        CourseManager.Instance.DisplayAllCourses();
        Console.WriteLine();

        Console.Write("Ingrese el CÓDIGO del curso al que desea inscribirse: ");
        string? codigo = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(codigo))
        {
            Console.WriteLine("❌ Código inválido");
        }
        else
        {
            var facade = new AcademicSystemFacade();
            facade.EnrollStudentInCourse(session.CurrentUser.Name, codigo);
        }

        PausarConsola();
    }

    static void CerrarSesion()
    {
        var session = UserSession.Instance;

        if (!session.IsLoggedIn)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️  No hay sesión activa");
            Console.ResetColor();
        }
        else
        {
            session.Logout();
        }

        PausarConsola();
    }

    static void VerCursos()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   CURSOS DISPONIBLES");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();

        CourseManager.Instance.DisplayAllCourses();

        PausarConsola();
    }

    static void DemostracionPatrones()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   DEMOSTRACIÓN DE PATRONES DE DISEÑO");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();

        Console.WriteLine("Seleccione qué patrones demostrar:");
        Console.WriteLine("  1. Patrones Creacionales (Prototype, Factory)");
        Console.WriteLine("  2. Patrones Estructurales (Adapter, Decorator, Composite)");
        Console.WriteLine("  3. Patrones Comportamentales (Strategy, Visitor, Iterator)");
        Console.WriteLine("  4. Todos los patrones");
        Console.Write("\nOpción: ");
        string? opcion = Console.ReadLine();

        Console.WriteLine();

        switch (opcion)
        {
            case "1":
                DemostrarPatronesCreacionales();
                break;
            case "2":
                DemostrarPatronesEstructurales();
                break;
            case "3":
                DemostrarPatronesComportamentales();
                break;
            case "4":
                DemostrarTodosLosPatrones();
                break;
            default:
                Console.WriteLine("Opción no válida");
                break;
        }

        PausarConsola();
    }

    static void DemostrarPatronesCreacionales()
    {
        Console.WriteLine("🔷 PATRONES CREACIONALES\n");

        // PROTOTYPE
        Console.WriteLine("--- PROTOTYPE ---");
        var profileRegistry = new ProfilePrototypeRegistry();
        var studentProfile = profileRegistry.GetPrototype("student");
        studentProfile.Name = "Perfil Clonado";
        studentProfile.DisplayProfile();
        Console.WriteLine();

        // OBJECT POOL
        Console.WriteLine("--- OBJECT POOL ---");
        var connectionPool = new ResourceOptimizer.ConnectionPool(3);
        var conn1 = connectionPool.AcquireConnection();
        var conn2 = connectionPool.AcquireConnection();
        conn1.ExecuteQuery("SELECT * FROM Students");
        connectionPool.ReleaseConnection(conn1);
        var conn3 = connectionPool.AcquireConnection(); // Reutiliza conn1
        connectionPool.ShowStatistics();
        Console.WriteLine();
    }

    static void DemostrarPatronesEstructurales()
    {
        Console.WriteLine("🔷 PATRONES ESTRUCTURALES\n");

        // ADAPTER
        Console.WriteLine("--- ADAPTER ---");
        var legacySystem = new LegacyConsoleSystem();
        IModernUI modernUI = new ConsoleAdapter(legacySystem);
        modernUI.RenderElement("Mensaje adaptado", "success");
        Console.WriteLine();

        // FACADE
        Console.WriteLine("--- FACADE ---");
        var facade = new AcademicSystemFacade();
        var summary = facade.GetSystemSummary();
        summary.Display();
        Console.WriteLine();

        // DECORATOR
        Console.WriteLine("--- DECORATOR ---");
        IUIElement text = new SimpleText("Texto decorado");
        IUIElement decorated = new BorderDecorator(new ColorDecorator(text, ConsoleColor.Green));
        decorated.Display();
        Console.WriteLine();

        // COMPOSITE
        Console.WriteLine("--- COMPOSITE ---");
        var panel = new UIContainer("Panel Principal");
        panel.Add(new UIElement("Elemento1", "Contenido 1"));
        panel.Add(new UIElement("Elemento2", "Contenido 2"));
        panel.Render();
        Console.WriteLine();
    }

    static void DemostrarPatronesComportamentales()
    {
        Console.WriteLine("🔷 PATRONES COMPORTAMENTALES\n");

        // STRATEGY
        Console.WriteLine("--- STRATEGY (Calificación) ---");
        var calculator = new GradeCalculator(new LetterGradingStrategy());
        Console.WriteLine($"Calificación: {calculator.Grade(85)}");
        calculator.SetStrategy(new PassFailGradingStrategy());
        Console.WriteLine($"Calificación: {calculator.Grade(85)}");
        Console.WriteLine();

        // INTERPRETER
        Console.WriteLine("--- INTERPRETER ---");
        var context = new Context();
        context.SetVariable("isStudent", true);
        context.SetVariable("hasEnrolled", true);
        AccessRuleInterpreter.EvaluateAccessRule("student_access", context);

        // NULL OBJECT
        Console.WriteLine("--- NULL OBJECT ---");
        var notificationService = new UserNotificationService();
        notificationService.RegisterUser("usuario1", true);
        notificationService.RegisterUser("usuario2", false);
        notificationService.NotifyUser("usuario1", "Tienes un nuevo mensaje");
        notificationService.NotifyUser("usuario2", "Tienes un nuevo mensaje");
        Console.WriteLine();

        // ITERATOR
        Console.WriteLine("--- ITERATOR ---");
        var students = new StudentCollection();
        students.AddStudent("Ana");
        students.AddStudent("Carlos");
        students.AddStudent("María");
        var iterator = students.CreateIterator();
        while (iterator.HasNext())
        {
            iterator.Next();
        }
        Console.WriteLine();

        // VISITOR
        Console.WriteLine("--- VISITOR ---");
        var studentRecord = new StudentRecord
        {
            Name = "Juan Pérez",
            Grades = new List<double> { 85, 90, 88 },
            Absences = 2
        };
        var statsVisitor = new StatisticsCalculatorVisitor();
        studentRecord.Accept(statsVisitor);
        Console.WriteLine();
    }

    static void DemostrarTodosLosPatrones()
    {
        DemostrarPatronesCreacionales();
        DemostrarPatronesEstructurales();
        DemostrarPatronesComportamentales();

        Console.WriteLine("\n✅ Demostración completa de patrones finalizada");
    }

    static bool ConfirmarSalida()
    {
        Console.Write("¿Está seguro que desea salir? (S/N): ");
        string? respuesta = Console.ReadLine();
        return respuesta?.ToUpper() == "S";
    }

    static void PausarConsola()
    {
        Console.WriteLine("\nPresione cualquier tecla para continuar...");
        Console.ReadKey(true);
    }

    static string LeerPasswordOculto()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
    static void MostrarEncabezado()
    {
        var session = UserSession.Instance;
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   SISTEMA DE GESTIÓN ACADÉMICA MODULAR");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();

        if (session.IsLoggedIn)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"👤 Usuario: {session.CurrentUser!.Name} ({session.CurrentUser.Role})");
            Console.WriteLine($"⏱️  Sesión activa: {session.SessionDuration.Minutes}m {session.SessionDuration.Seconds}s");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️  No hay sesión activa");
            Console.ResetColor();
        }

        Console.WriteLine();
    }

    static void GestionarContenidoCurso(Course course)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine($"   GESTIÓN DE CONTENIDO: {course.Name}");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine($"\n📚 Curso: {course.Name} ({course.Code})");
        Console.WriteLine($"   Créditos: {course.Credits}");
        Console.WriteLine($"   Materiales actuales: {course.TheoryContent.Count + course.PracticeContent.Count}");
        
        Console.WriteLine("\nFuncionalidades disponibles:");
        Console.WriteLine("1. Subir materiales (PDF, Video)");
        Console.WriteLine("2. Ver materiales existentes");
        Console.WriteLine("3. Eliminar materiales");
        
        Console.WriteLine("\n⚠️  Funcionalidad en desarrollo para el prototipo v2.0");
        PausarConsola();
    }

    static void VerMisCursos()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   MIS CURSOS INSCRITOS (ESTUDIANTE)");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();

        var session = UserSession.Instance;
        if (session.CurrentUser is Student student)
        {
            if (student.EnrolledCourses.Count == 0)
            {
                Console.WriteLine("\nNo estás inscrito en ningún curso.");
            }
            else
            {
                Console.WriteLine("\nCursos actuales:");
                foreach (var courseCode in student.EnrolledCourses)
                {
                    var course = CourseManager.Instance.FindCourse(courseCode);
                    if (course != null)
                    {
                        Console.WriteLine($"✅ [{course.Code}] {course.Name} - Prof. {course.Instructor}");
                    }
                    else
                    {
                        Console.WriteLine($"❓ [{courseCode}] Curso no encontrado");
                    }
                }
            }
        }
        
        PausarConsola();
    }

    static void GestionarUsuarios()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   GESTIÓN DE USUARIOS (ADMINISTRADOR)");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();

        Console.WriteLine("\n👥 Lista de usuarios registrados:");
        var allUsers = UserSession.GetAllUsers();
        int count = 1;
        foreach (var user in allUsers)
        {
            Console.WriteLine($"{count}. {user.Name} ({user.Role}) - {user.Email}");
            count++;
        }

        Console.WriteLine("\n🛠️  Opciones de gestión:");
        Console.WriteLine("  1. Crear nuevo usuario");
        Console.WriteLine("  2. Ver estadísticas de usuarios");
        Console.WriteLine("  3. Volver");
        Console.Write("\nSeleccione opción (1-3): ");
        string? opcion = Console.ReadLine();
        
        switch (opcion)
        {
            case "1":
                RegistrarUsuario();
                return;
            case "2":
                Console.WriteLine($"\n📊 Estadísticas:");
                Console.WriteLine($"   Total de usuarios: {allUsers.Count}");
                Console.WriteLine($"   Administradores: {allUsers.Count(u => u.Role == "Administrador")}");
                Console.WriteLine($"   Profesores: {allUsers.Count(u => u.Role == "Profesor")}");
                Console.WriteLine($"   Estudiantes: {allUsers.Count(u => u.Role == "Estudiante")}");
                break;
            case "3":
                return;
            default:
                Console.WriteLine("\n❌ Opción no válida");
                break;
        }
        
        PausarConsola();
    }

    // ==================== MÉTODOS PARA ESTUDIANTE ====================
    
    static void MisCursos()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   MIS CURSOS INSCRITOS");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();

        var session = UserSession.Instance;
        if (session.CurrentUser is Student student)
        {
            if (student.EnrolledCourses.Count == 0)
            {
                Console.WriteLine("\n📚 No estás inscrito en ningún curso.");
                Console.WriteLine("\n💡 Cursos disponibles:");
                CourseManager.Instance.DisplayAllCourses();
                
                Console.WriteLine("\n¿Deseas inscribirte en un curso? (S/N): ");
                string? respuesta = Console.ReadLine();
                if (respuesta?.ToUpper() == "S")
                {
                    Console.Write("Ingrese el código del curso: ");
                    string? codigo = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(codigo))
                    {
                        var facade = new AcademicSystemFacade();
                        facade.EnrollStudentInCourse(session.CurrentUser.Name, codigo);
                    }
                }
            }
            else
            {
                Console.WriteLine("\n📚 Cursos actuales:");
                foreach (var courseCode in student.EnrolledCourses)
                {
                    var course = CourseManager.Instance.FindCourse(courseCode);
                    if (course != null)
                    {
                        Console.WriteLine($"\n  ✅ [{course.Code}] {course.Name}");
                        Console.WriteLine($"     Profesor: {course.Instructor}");
                        Console.WriteLine($"     Créditos: {course.Credits}");
                        Console.WriteLine($"     Descripción: {course.Description}");
                    }
                }
            }
        }
        
        PausarConsola();
    }

    static void DescargarMateriales()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   DESCARGAR MATERIALES");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        var session = UserSession.Instance;
        if (session.CurrentUser is Student student)
        {
            if (student.EnrolledCourses.Count == 0)
            {
                Console.WriteLine("\n❌ No estás inscrito en ningún curso.");
            }
            else
            {
                Console.WriteLine("\n📥 Materiales disponibles por curso:\n");
                foreach (var courseCode in student.EnrolledCourses)
                {
                    var course = CourseManager.Instance.FindCourse(courseCode);
                    if (course != null)
                    {
                        Console.WriteLine($"📚 {course.Name} [{course.Code}]");
                        Console.WriteLine("   Contenido teórico:");
                        foreach (var content in course.TheoryContent)
                        {
                            Console.WriteLine($"   • {content.Title} ({content.Type})");
                        }
                        Console.WriteLine("   Contenido práctico:");
                        foreach (var content in course.PracticeContent)
                        {
                            Console.WriteLine($"   • {content.Title} ({content.Type})");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
        
        PausarConsola();
    }

    static void EntregarActividades()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   ENTREGAR ACTIVIDADES");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        var session = UserSession.Instance;
        if (session.CurrentUser is Student student)
        {
            if (student.EnrolledCourses.Count == 0)
            {
                Console.WriteLine("\n❌ No estás inscrito en ningún curso.");
            }
            else
            {
                Console.WriteLine("\n📝 Actividades pendientes:\n");
                int activityCount = 1;
                
                foreach (var courseCode in student.EnrolledCourses)
                {
                    var course = CourseManager.Instance.FindCourse(courseCode);
                    if (course != null)
                    {
                        Console.WriteLine($"📚 {course.Name}");
                        Console.WriteLine($"   {activityCount}. Tarea: Investigación del tema");
                        Console.WriteLine($"      Fecha límite: {DateTime.Now.AddDays(7):dd/MM/yyyy}");
                        activityCount++;
                        Console.WriteLine($"   {activityCount}. Examen: Evaluación parcial");
                        Console.WriteLine($"      Fecha límite: {DateTime.Now.AddDays(14):dd/MM/yyyy}");
                        activityCount++;
                        Console.WriteLine();
                    }
                }
                
                Console.Write("\n¿Deseas entregar una actividad? (S/N): ");
                if (Console.ReadLine()?.ToUpper() == "S")
                {
                    Console.Write("Número de actividad: ");
                    if (int.TryParse(Console.ReadLine(), out int num))
                    {
                        Console.Write("Escribe tu respuesta/comentario: ");
                        string? respuesta = Console.ReadLine();
                        
                        Console.WriteLine("\n✅ Actividad entregada exitosamente");
                        Console.WriteLine($"   Fecha de entrega: {DateTime.Now:dd/MM/yyyy HH:mm}");
                        Console.WriteLine($"   Estado: Pendiente de calificación");
                        
                        NotificationService.Instance.NotifyUserLogin("Profesor", $"Nueva entrega de {student.Name}");
                    }
                }
            }
        }
        
        PausarConsola();
    }

    static void ConsultarCalificaciones()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   CONSULTAR CALIFICACIONES");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        var session = UserSession.Instance;
        if (session.CurrentUser is Student student)
        {
            if (student.EnrolledCourses.Count == 0)
            {
                Console.WriteLine("\n❌ No estás inscrito en ningún curso.");
            }
            else
            {
                Console.WriteLine("\n📊 Calificaciones por curso:\n");
                
                // Usar Strategy Pattern para calificaciones
                var letterGrading = new GradeCalculator(new LetterGradingStrategy());
                
                foreach (var courseCode in student.EnrolledCourses)
                {
                    var course = CourseManager.Instance.FindCourse(courseCode);
                    if (course != null)
                    {
                        Console.WriteLine($"📚 {course.Name} [{course.Code}]");
                        
                        // Generar calificaciones de ejemplo
                        double nota = new Random().Next(60, 100);
                        Console.WriteLine($"   Calificación: {letterGrading.Grade(nota)}");
                        Console.WriteLine($"   Nota: {nota}/100");
                        Console.WriteLine($"   Progreso: {new Random().Next(50, 100)}%");
                        Console.WriteLine();
                    }
                }
                
                Console.WriteLine("🎯 Promedio general: " + new Random().Next(70, 95) + "/100");
            }
        }
        
        PausarConsola();
    }

    static void Mensajes()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   MENSAJES Y COMUNICACIÓN");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine("\n💬 Opciones:");
        Console.WriteLine("  1. Ver bandeja de entrada");
        Console.WriteLine("  2. Enviar mensaje a profesor");
        Console.WriteLine("  3. Ver anuncios del curso");
        Console.Write("\nSeleccione opción (1-3): ");
        string? opcion = Console.ReadLine();
        
        switch (opcion)
        {
            case "1":
                Console.WriteLine("\n📬 Bandeja de entrada:");
                Console.WriteLine("  1. [Profesor1] Re: Consulta sobre tarea - 02/12/2025");
                Console.WriteLine("  2. [Sistema] Recordatorio: Examen próximo - 01/12/2025");
                Console.WriteLine("  3. [Profesor2] Calificación publicada - 30/11/2025");
                break;
            case "2":
                Console.Write("\nProfesor destinatario: ");
                string? profesor = Console.ReadLine();
                Console.Write("Asunto: ");
                string? asunto = Console.ReadLine();
                Console.Write("Mensaje: ");
                string? mensaje = Console.ReadLine();
                
                Console.WriteLine("\n✅ Mensaje enviado exitosamente");
                NotificationService.Instance.NotifyUserLogin(profesor ?? "Profesor", "Nuevo mensaje recibido");
                break;
            case "3":
                Console.WriteLine("\n📢 Anuncios recientes:");
                Console.WriteLine("  • Cambio de horario para el examen final");
                Console.WriteLine("  • Nuevos materiales disponibles en el curso");
                Console.WriteLine("  • Recordatorio: Entrega de proyecto final");
                break;
            default:
                Console.WriteLine("\n❌ Opción no válida");
                break;
        }
        
        PausarConsola();
    }

    static void Tramites()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   TRÁMITES ACADÉMICOS");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        var session = UserSession.Instance;
        if (session.CurrentUser is Student student)
        {
            Console.WriteLine("\n📄 Trámites disponibles:");
            Console.WriteLine("  1. Generar constancia de inscripción");
            Console.WriteLine("  2. Ver horario de clases");
            Console.WriteLine("  3. Consultar record académico");
            Console.WriteLine("  4. Solicitar certificado");
            Console.Write("\nSeleccione opción (1-4): ");
            string? opcion = Console.ReadLine();
            
            switch (opcion)
            {
                case "1":
                    Console.WriteLine("\n📜 CONSTANCIA DE INSCRIPCIÓN");
                    Console.WriteLine("═══════════════════════════════════════");
                    Console.WriteLine($"Estudiante: {student.Name}");
                    Console.WriteLine($"ID: {student.StudentId}");
                    Console.WriteLine($"Cursos inscritos: {student.EnrolledCourses.Count}");
                    Console.WriteLine($"Fecha: {DateTime.Now:dd/MM/yyyy}");
                    Console.WriteLine("\n✅ Constancia generada (simulada)");
                    break;
                case "2":
                    Console.WriteLine("\n📅 HORARIO DE CLASES");
                    Console.WriteLine("═══════════════════════════════════════");
                    foreach (var courseCode in student.EnrolledCourses)
                    {
                        var course = CourseManager.Instance.FindCourse(courseCode);
                        if (course != null)
                        {
                            Console.WriteLine($"\n{course.Name}");
                            Console.WriteLine("  Lunes: 08:00 - 10:00");
                            Console.WriteLine("  Miércoles: 08:00 - 10:00");
                        }
                    }
                    break;
                case "3":
                    Console.WriteLine("\n📊 RECORD ACADÉMICO");
                    Console.WriteLine("═══════════════════════════════════════");
                    Console.WriteLine($"Estudiante: {student.Name}");
                    Console.WriteLine($"Cursos completados: 0");
                    Console.WriteLine($"Cursos en curso: {student.EnrolledCourses.Count}");
                    Console.WriteLine($"Promedio general: N/A");
                    break;
                case "4":
                    Console.WriteLine("\n✅ Solicitud de certificado enviada");
                    Console.WriteLine("   Tiempo estimado: 5 días hábiles");
                    break;
                default:
                    Console.WriteLine("\n❌ Opción no válida");
                    break;
            }
        }
        
        PausarConsola();
    }

    // ==================== MÉTODOS PARA PROFESOR ====================

    static void CrearActividades(Course course)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine($"   CREAR ACTIVIDADES: {course.Name}");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine($"\n📚 Curso: {course.Name} ({course.Code})");
        
        Console.WriteLine("\n📝 Tipos de actividades:");
        Console.WriteLine("  1. Crear tarea");
        Console.WriteLine("  2. Crear examen");
        Console.WriteLine("  3. Crear práctica");
        Console.WriteLine("  4. Crear cuestionario");
        Console.Write("\nSeleccione tipo (1-4): ");
        string? tipo = Console.ReadLine();
        
        string tipoActividad = tipo switch
        {
            "1" => "Tarea",
            "2" => "Examen",
            "3" => "Práctica",
            "4" => "Cuestionario",
            _ => "Tarea"
        };
        
        Console.Write($"\nTítulo de la {tipoActividad}: ");
        string? titulo = Console.ReadLine();
        
        Console.Write("Descripción: ");
        string? descripcion = Console.ReadLine();
        
        Console.WriteLine($"\n✅ {tipoActividad} '{titulo}' creada exitosamente para {course.Name}");
        Console.WriteLine($"   Descripción: {descripcion}");
        
        // Notificar creación
        NotificationService.Instance.NotifyCourseCreated($"{tipoActividad}: {titulo}", "ACT-" + DateTime.Now.Ticks);
        
        PausarConsola();
    }

    static void EvaluarEstudiantes(Course course)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine($"   EVALUAR ESTUDIANTES: {course.Name}");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine($"\n📚 Curso: {course.Name} ({course.Code})");
        
        // Paso 1: Seleccionar tipo de actividad
        Console.WriteLine("\n📝 TIPOS DE ACTIVIDADES:");
        Console.WriteLine("  1. Tarea");
        Console.WriteLine("  2. Examen");
        Console.WriteLine("  3. Práctica");
        Console.WriteLine("  4. Cuestionario");
        Console.Write("\nSeleccione tipo de actividad (1-4): ");
        string? tipoOpcion = Console.ReadLine();
        
        string tipoActividad = tipoOpcion switch
        {
            "1" => "Tarea",
            "2" => "Examen",
            "3" => "Práctica",
            "4" => "Cuestionario",
            _ => "Tarea"
        };
        
        // Mostrar actividades simuladas de ese tipo
        Console.WriteLine($"\n📋 Actividades de tipo '{tipoActividad}':");
        Console.WriteLine($"  1. {tipoActividad} 1: Introducción al tema");
        Console.WriteLine($"  2. {tipoActividad} 2: Desarrollo práctico");
        Console.WriteLine($"  3. {tipoActividad} 3: Evaluación final");
        Console.Write("\nSeleccione actividad a calificar (1-3): ");
        string? actividadOpcion = Console.ReadLine();
        
        string actividadNombre = actividadOpcion switch
        {
            "1" => $"{tipoActividad} 1: Introducción al tema",
            "2" => $"{tipoActividad} 2: Desarrollo práctico",
            "3" => $"{tipoActividad} 3: Evaluación final",
            _ => $"{tipoActividad} 1: Introducción al tema"
        };
        
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine($"   EVALUAR: {actividadNombre}");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine($"\n📚 Curso: {course.Name} ({course.Code})");
        
        // Paso 2: Seleccionar sistema de calificación
        Console.WriteLine("\n📊 Opciones de evaluación:");
        Console.WriteLine("  1. Calificar con letras (A, B, C, D, F)");
        Console.WriteLine("  2. Calificar numéricamente (0-100)");
        Console.WriteLine("  3. Aprobar/Reprobar");
        Console.Write("\nSeleccione sistema de calificación (1-3): ");
        string? opcion = Console.ReadLine();
        
        IGradingStrategy strategy = opcion switch
        {
            "1" => new LetterGradingStrategy(),
            "2" => new LetterGradingStrategy(), // Usar LetterGrading para numérico también
            "3" => new PassFailGradingStrategy(),
            _ => new LetterGradingStrategy()
        };
        
        var calculator = new GradeCalculator(strategy);
        Console.WriteLine($"\n[STRATEGY] Calculadora configurada con: {strategy.GetStrategyName()}");
        
        // Paso 3: Listar estudiantes con estado de entrega
        var allUsers = UserSession.GetAllUsers();
        var students = allUsers.Where(u => u.Role == "Estudiante").ToList();
        
        Console.WriteLine("\n👥 ESTUDIANTES INSCRITOS:\n");
        
        if (students.Count == 0)
        {
            Console.WriteLine("❌ No hay estudiantes inscritos en este curso.");
            PausarConsola();
            return;
        }
        
        // Simular estado de entrega (en producción vendría de BD)
        var random = new Random();
        var estudiantesConEstado = students.Select(s => new
        {
            Student = s,
            HaEntregado = random.Next(0, 2) == 1, // 50% probabilidad
            Calificacion = (double?)null
        }).ToList();
        
        for (int i = 0; i < estudiantesConEstado.Count; i++)
        {
            var item = estudiantesConEstado[i];
            string estado = item.HaEntregado ? "✅ Entregado" : "❌ No entregado";
            string calificacion = item.Calificacion.HasValue ? $"[{item.Calificacion}]" : "[Sin calificar]";
            
            Console.WriteLine($"  {i + 1}. {item.Student.Name} - {estado} {calificacion}");
        }
        
        // Paso 4: Calificar estudiantes
        Console.WriteLine("\n📝 CALIFICAR ESTUDIANTES:");
        Console.Write("\n¿Desea calificar estudiantes? (S/N): ");
        if (Console.ReadLine()?.ToUpper() != "S")
        {
            PausarConsola();
            return;
        }
        
        foreach (var item in estudiantesConEstado.Where(e => e.HaEntregado))
        {
            Console.WriteLine($"\n👤 Estudiante: {item.Student.Name}");
            Console.Write("   Ingrese la nota (0-100) o [Enter] para omitir: ");
            string? notaInput = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(notaInput))
            {
                Console.WriteLine("   ⏭️  Omitido");
                continue;
            }
            
            if (double.TryParse(notaInput, out double nota))
            {
                string resultado = calculator.Grade(nota);
                Console.Write("   Retroalimentación (opcional): ");
                string? feedback = Console.ReadLine();
                
                Console.WriteLine($"   ✅ Calificación: {resultado}");
                if (!string.IsNullOrWhiteSpace(feedback))
                {
                    Console.WriteLine($"   💬 Feedback: {feedback}");
                }
                
                // Notificar al estudiante
                NotificationService.Instance.NotifyUserLogin(item.Student.Name, 
                    $"Nueva calificación en {actividadNombre}: {resultado}");
            }
            else
            {
                Console.WriteLine("   ❌ Nota inválida - Omitido");
            }
        }
        
        Console.WriteLine("\n✅ Proceso de calificación completado");
        PausarConsola();
    }

    static void ControlAsistencia(Course course)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine($"   CONTROL DE ASISTENCIA: {course.Name}");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine($"\n📚 Curso: {course.Name} ({course.Code})");
        
        Console.WriteLine("\n📋 Opciones:");
        Console.WriteLine("  1. Registrar asistencia de hoy");
        Console.WriteLine("  2. Ver historial de asistencia");
        Console.WriteLine("  3. Generar reporte");
        Console.Write("\nSeleccione opción (1-3): ");
        string? opcion = Console.ReadLine();
        
        switch (opcion)
        {
            case "1":
                Console.WriteLine($"\n📅 Fecha: {DateTime.Now:dd/MM/yyyy}");
                Console.WriteLine("\nEstudiantes:");
                var allUsers = UserSession.GetAllUsers();
                int count = 1;
                foreach (var user in allUsers.Where(u => u.Role == "Estudiante"))
                {
                    Console.Write($"{count}. {user.Name} - [P]resente / [A]usente / [T]ardanza: ");
                    string? estado = Console.ReadLine()?.ToUpper();
                    string estadoTexto = estado switch
                    {
                        "P" => "✅ Presente",
                        "A" => "❌ Ausente",
                        "T" => "⏰ Tardanza",
                        _ => "❓ No registrado"
                    };
                    Console.WriteLine($"   {estadoTexto}");
                    count++;
                }
                Console.WriteLine("\n✅ Asistencia registrada exitosamente");
                break;
            case "2":
                Console.WriteLine("\n📊 Historial de asistencia:");
                Console.WriteLine($"Fecha: {DateTime.Now.AddDays(-1):dd/MM/yyyy} - 85% asistencia");
                Console.WriteLine($"Fecha: {DateTime.Now.AddDays(-2):dd/MM/yyyy} - 90% asistencia");
                Console.WriteLine($"Fecha: {DateTime.Now.AddDays(-3):dd/MM/yyyy} - 88% asistencia");
                break;
            case "3":
                Console.WriteLine("\n📊 REPORTE DE ASISTENCIA");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("Promedio general: 87.6%");
                Console.WriteLine("Total de clases: 15");
                Console.WriteLine("Estudiantes con más del 80%: 8/10");
                break;
            default:
                Console.WriteLine("\n❌ Opción no válida");
                break;
        }
        
        PausarConsola();
    }

    static void ComunicacionProfesor(Course course)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine($"   COMUNICACIÓN CON ESTUDIANTES: {course.Name}");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine($"\n📚 Curso: {course.Name} ({course.Code})");
        
        Console.WriteLine("\n💬 Opciones:");
        Console.WriteLine("  1. Enviar anuncio al curso");
        Console.WriteLine("  2. Ver mensajes recibidos");
        Console.WriteLine("  3. Responder consultas");
        Console.Write("\nSeleccione opción (1-3): ");
        string? opcion = Console.ReadLine();
        
        switch (opcion)
        {
            case "1":
                Console.Write("\nTítulo del anuncio: ");
                string? titulo = Console.ReadLine();
                Console.Write("Mensaje: ");
                string? mensaje = Console.ReadLine();
                
                Console.WriteLine("\n✅ Anuncio publicado exitosamente");
                Console.WriteLine($"   Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                Console.WriteLine("   Notificaciones enviadas a todos los estudiantes");
                
                NotificationService.Instance.NotifyCourseCreated(titulo ?? "Anuncio", "ANN-" + DateTime.Now.Ticks);
                break;
            case "2":
                Console.WriteLine("\n📨 Mensajes recibidos:");
                Console.WriteLine("  1. [Estudiante1] Consulta sobre tarea - Hoy 10:30");
                Console.WriteLine("  2. [Estudiante2] Solicitud de prórroga - Ayer 15:45");
                Console.WriteLine("  3. [Estudiante3] Duda sobre examen - 01/12 09:20");
                break;
            case "3":
                Console.WriteLine("\n❓ Consultas pendientes:");
                Console.WriteLine("  1. ¿Cómo se calcula el promedio final?");
                Console.WriteLine("  2. ¿Puedo entregar la tarea después de la fecha?");
                Console.Write("\nNúmero de consulta a responder: ");
                if (int.TryParse(Console.ReadLine(), out int num))
                {
                    Console.Write("Respuesta: ");
                    string? respuesta = Console.ReadLine();
                    Console.WriteLine("\n✅ Respuesta enviada");
                }
                break;
            default:
                Console.WriteLine("\n❌ Opción no válida");
                break;
        }
        
        PausarConsola();
    }

    static void MonitoreoAvance(Course course)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine($"   MONITOREO DEL AVANCE: {course.Name}");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine($"\n📚 Curso: {course.Name} ({course.Code})");
        
        Console.WriteLine("\n📈 Estadísticas del curso:\n");
        
        var allUsers = UserSession.GetAllUsers();
        var students = allUsers.Where(u => u.Role == "Estudiante").ToList();
        
        Console.WriteLine($"Total de estudiantes: {students.Count}");
        Console.WriteLine($"Promedio general del curso: {new Random().Next(70, 90)}/100");
        Console.WriteLine($"Tasa de aprobación: {new Random().Next(75, 95)}%");
        Console.WriteLine();
        
        Console.WriteLine("👥 Desempeño por estudiante:");
        foreach (var student in students.Take(5))
        {
            int promedio = new Random().Next(60, 100);
            string estado = promedio >= 70 ? "✅" : "⚠️";
            Console.WriteLine($"  {estado} {student.Name}: {promedio}/100");
        }
        
        Console.WriteLine("\n🚨 Estudiantes en riesgo:");
        Console.WriteLine("  • Estudiante con promedio < 70: 2");
        Console.WriteLine("  • Estudiante con asistencia < 80%: 1");
        Console.WriteLine("  • Actividades sin entregar: 3");
        
        Console.WriteLine("\n📊 Progreso de actividades:");
        Console.WriteLine("  • Tarea 1: 8/10 entregas (80%)");
        Console.WriteLine("  • Examen parcial: 10/10 entregas (100%)");
        Console.WriteLine("  • Práctica 1: 6/10 entregas (60%)");
        
        PausarConsola();
    }

    // ==================== MÉTODOS PARA ADMINISTRADOR ====================

    static void GestionarCursos()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   GESTIÓN DE CURSOS");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine("\n📚 Opciones de gestión:");
        Console.WriteLine("  1. Ver todos los cursos");
        Console.WriteLine("  2. Crear nuevo curso");
        Console.WriteLine("  3. Asignar profesor a curso");
        Console.Write("\nSeleccione opción (1-3): ");
        string? opcion = Console.ReadLine();
        
        switch (opcion)
        {
            case "1":
                CourseManager.Instance.DisplayAllCourses();
                break;
            case "2":
                CrearCurso();
                return;
            case "3":
                Console.WriteLine("\n👨‍🏫 ASIGNAR PROFESOR A CURSO");
                Console.WriteLine("═══════════════════════════════════════");
                
                var courses = CourseManager.Instance.GetAllCourses();
                if (courses.Count == 0)
                {
                    Console.WriteLine("❌ No hay cursos disponibles.");
                    break;
                }

                Console.WriteLine("\n📚 Cursos disponibles:");
                foreach (var c in courses)
                {
                    Console.WriteLine($"  [{c.Code}] {c.Name} (Instructor actual: {(string.IsNullOrEmpty(c.Instructor) ? "Sin asignar" : c.Instructor)})");
                }

                Console.Write("\nIngrese el código del curso: ");
                string? code = Console.ReadLine();
                var course = CourseManager.Instance.FindCourse(code ?? "");

                if (course == null)
                {
                    Console.WriteLine("❌ Curso no encontrado.");
                    break;
                }

                var professors = UserSession.GetAllUsers().Where(u => u.Role == "Profesor").ToList();
                if (professors.Count == 0)
                {
                    Console.WriteLine("❌ No hay profesores registrados en el sistema.");
                    break;
                }

                Console.WriteLine("\n👨‍🏫 Profesores disponibles:");
                for (int i = 0; i < professors.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {professors[i].Name} ({professors[i].Email})");
                }

                Console.Write("\nSeleccione el número del profesor: ");
                if (int.TryParse(Console.ReadLine(), out int profIndex) && profIndex > 0 && profIndex <= professors.Count)
                {
                    var selectedProf = professors[profIndex - 1];
                    course.Instructor = selectedProf.Name;
                    Console.WriteLine($"\n✅ Profesor {selectedProf.Name} asignado correctamente al curso {course.Code}.");
                    
                    // Notificar al profesor (simulado)
                    NotificationService.Instance.NotifyUserLogin(selectedProf.Name, $"Has sido asignado al curso {course.Name}");
                }
                else
                {
                    Console.WriteLine("❌ Selección inválida.");
                }
                break;
            default:
                Console.WriteLine("\n❌ Opción no válida");
                break;
        }
        
        PausarConsola();
    }

    static void GestionAcademica()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   GESTIÓN ACADÉMICA");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine("\n🎓 Opciones:");
        Console.WriteLine("  1. Gestionar periodos escolares");
        Console.WriteLine("  2. Administrar grupos");
        Console.WriteLine("  3. Gestionar carreras");
        Console.WriteLine("  4. Ver estadísticas generales");
        Console.Write("\nSeleccione opción (1-4): ");
        string? opcion = Console.ReadLine();
        
        switch (opcion)
        {
            case "1":
                Console.WriteLine("\n📅 PERIODOS ESCOLARES");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("Periodos activos:");
                Console.WriteLine("  • 2025-1: Enero - Junio 2025 (Activo)");
                Console.WriteLine("  • 2025-2: Julio - Diciembre 2025 (Próximo)");
                Console.WriteLine("\n¿Desea crear un nuevo periodo? (S/N): ");
                if (Console.ReadLine()?.ToUpper() == "S")
                {
                    Console.Write("Nombre del periodo: ");
                    string? nombre = Console.ReadLine();
                    Console.WriteLine("✅ Periodo creado exitosamente");
                }
                break;
            case "2":
                Console.WriteLine("\n👥 GRUPOS");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("Grupos existentes:");
                Console.WriteLine("  • Grupo A - Turno Matutino (25 estudiantes)");
                Console.WriteLine("  • Grupo B - Turno Vespertino (22 estudiantes)");
                Console.WriteLine("  • Grupo C - Turno Nocturno (18 estudiantes)");
                break;
            case "3":
                Console.WriteLine("\n🎓 CARRERAS");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("Carreras disponibles:");
                Console.WriteLine("  • Ingeniería en Sistemas Computacionales");
                Console.WriteLine("  • Ingeniería en Tecnologías de la Información");
                Console.WriteLine("  • Licenciatura en Informática");
                break;
            case "4":
                Console.WriteLine("\n📊 ESTADÍSTICAS GENERALES");
                Console.WriteLine("═══════════════════════════════════════");
                var allUsers = UserSession.GetAllUsers();
                Console.WriteLine($"Total de usuarios: {allUsers.Count}");
                Console.WriteLine($"Total de cursos: {CourseManager.Instance.GetAllCourses().Count}");
                Console.WriteLine($"Estudiantes activos: {allUsers.Count(u => u.Role == "Estudiante")}");
                Console.WriteLine($"Profesores activos: {allUsers.Count(u => u.Role == "Profesor")}");
                break;
            default:
                Console.WriteLine("\n❌ Opción no válida");
                break;
        }
        
        PausarConsola();
    }

    static void VerReportesEstadisticas()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   REPORTES Y ESTADÍSTICAS");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine("\n📊 Información disponible:");
        Console.WriteLine("  • Monitorear estadísticas generales");
        Console.WriteLine("  • Ver cumplimiento de actividades");
        Console.WriteLine("  • Reportes de desempeño");
        Console.WriteLine("  • Análisis de datos académicos");
        
        ReportService.GenerateSystemReport();
        
        PausarConsola();
    }

    static void ConfiguracionSistema()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("   CONFIGURACIÓN DEL SISTEMA");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine("\n⚙️  Opciones:");
        Console.WriteLine("  1. Ver información del sistema");
        Console.WriteLine("  2. Configurar parámetros");
        Console.WriteLine("  3. Seguridad y respaldos");
        Console.WriteLine("  4. Mantenimiento");
        Console.Write("\nSeleccione opción (1-4): ");
        string? opcion = Console.ReadLine();
        
        switch (opcion)
        {
            case "1":
                Console.WriteLine("\n💻 INFORMACIÓN DEL SISTEMA");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine($"Versión: 1.0.0");
                Console.WriteLine($"Framework: .NET 8.0");
                Console.WriteLine($"Patrones implementados: 23 GoF");
                Console.WriteLine($"Fecha de inicio: {DateTime.Now:dd/MM/yyyy}");
                Console.WriteLine($"Usuarios registrados: {UserSession.GetAllUsers().Count}");
                Console.WriteLine($"Cursos activos: {CourseManager.Instance.GetAllCourses().Count}");
                break;
            case "2":
                Console.WriteLine("\n⚙️  PARÁMETROS DEL SISTEMA");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("  • Tiempo de sesión: 30 minutos");
                Console.WriteLine("  • Máximo de intentos de login: 3");
                Console.WriteLine("  • Tamaño máximo de archivo: 10 MB");
                Console.WriteLine("  • Idioma: Español");
                break;
            case "3":
                Console.WriteLine("\n🔒 SEGURIDAD Y RESPALDOS");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("  • Último respaldo: Hoy 02:00 AM");
                Console.WriteLine("  • Frecuencia de respaldo: Diario");
                Console.WriteLine("  • Encriptación: Activa");
                Console.WriteLine("  • Autenticación de dos factores: Desactivada");
                break;
            case "4":
                Console.WriteLine("\n🛠️  MANTENIMIENTO");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("  • Limpiar caché");
                Console.WriteLine("  • Optimizar base de datos");
                Console.WriteLine("  • Ver logs del sistema");
                Console.WriteLine("  • Verificar integridad de archivos");
                Console.WriteLine("\n⚠️  Opciones de mantenimiento en desarrollo");
                break;
            default:
                Console.WriteLine("\n❌ Opción no válida");
                break;
        }
        
        PausarConsola();
    }
}

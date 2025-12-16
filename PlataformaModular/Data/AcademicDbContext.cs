using Microsoft.EntityFrameworkCore;
using PlataformaAcademicaModular.Data.Entities;

namespace PlataformaAcademicaModular.Data;

/// <summary>
/// DbContext principal del sistema académico
/// Implementa patrón Repository a través de DbSet
/// </summary>
public class AcademicDbContext : DbContext
{
    public AcademicDbContext(DbContextOptions<AcademicDbContext> options) : base(options)
    {
    }

    // DbSets - Representan las tablas
    public DbSet<UsuarioEntity> Usuarios { get; set; }
    public DbSet<CursoEntity> Cursos { get; set; }
    public DbSet<InscripcionEntity> Inscripciones { get; set; }
    public DbSet<MaterialEntity> Materiales { get; set; }
    public DbSet<ActividadEntity> Actividades { get; set; }
    public DbSet<EntregaEntity> Entregas { get; set; }
    public DbSet<CalificacionEntity> Calificaciones { get; set; }
    public DbSet<MensajeEntity> Mensajes { get; set; }
    public DbSet<NotificacionEntity> Notificaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de relaciones y restricciones

        // Usuario - Inscripciones
        modelBuilder.Entity<InscripcionEntity>()
            .HasOne(i => i.Student)
            .WithMany(u => u.Inscripciones)
            .HasForeignKey(i => i.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Curso - Inscripciones
        modelBuilder.Entity<InscripcionEntity>()
            .HasOne(i => i.Course)
            .WithMany(c => c.Inscripciones)
            .HasForeignKey(i => i.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Curso - Instructor
        modelBuilder.Entity<CursoEntity>()
            .HasOne(c => c.Instructor)
            .WithMany(u => u.CursosImpartidos)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.SetNull);

        // Curso - Materiales
        modelBuilder.Entity<MaterialEntity>()
            .HasOne(m => m.Course)
            .WithMany(c => c.Materiales)
            .HasForeignKey(m => m.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Curso - Actividades
        modelBuilder.Entity<ActividadEntity>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Actividades)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Actividad - Entregas
        modelBuilder.Entity<EntregaEntity>()
            .HasOne(e => e.Activity)
            .WithMany(a => a.Entregas)
            .HasForeignKey(e => e.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Entrega - Calificación (1 a 1)
        modelBuilder.Entity<CalificacionEntity>()
            .HasOne(c => c.Entrega)
            .WithOne(e => e.Calificacion)
            .HasForeignKey<CalificacionEntity>(c => c.EntregaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Mensajes - Sender/Receiver
        modelBuilder.Entity<MensajeEntity>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MensajeEntity>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Cascade);

        // Notificaciones - Usuario
        modelBuilder.Entity<NotificacionEntity>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices únicos
        modelBuilder.Entity<UsuarioEntity>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<UsuarioEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<CursoEntity>()
            .HasIndex(c => c.Code)
            .IsUnique();

        modelBuilder.Entity<InscripcionEntity>()
            .HasIndex(i => new { i.StudentId, i.CourseId })
            .IsUnique();

        modelBuilder.Entity<EntregaEntity>()
            .HasIndex(e => new { e.ActivityId, e.StudentId })
            .IsUnique();
    }
}

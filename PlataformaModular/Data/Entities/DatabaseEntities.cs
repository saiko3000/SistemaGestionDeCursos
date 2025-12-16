using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaAcademicaModular.Data.Entities;

[Table("usuarios")]
public class UsuarioEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("username")]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("password_hash")]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [Column("email")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("role")]
    [MaxLength(20)]
    public string Role { get; set; } = string.Empty;

    [Column("additional_info")]
    [MaxLength(255)]
    public string? AdditionalInfo { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual ICollection<InscripcionEntity> Inscripciones { get; set; } = new List<InscripcionEntity>();
    public virtual ICollection<CursoEntity> CursosImpartidos { get; set; } = new List<CursoEntity>();
}

[Table("cursos")]
public class CursoEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("code")]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [Column("name")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("credits")]
    public int Credits { get; set; } = 3;

    [Column("instructor_id")]
    public int? InstructorId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("InstructorId")]
    public virtual UsuarioEntity? Instructor { get; set; }
    
    public virtual ICollection<InscripcionEntity> Inscripciones { get; set; } = new List<InscripcionEntity>();
    public virtual ICollection<MaterialEntity> Materiales { get; set; } = new List<MaterialEntity>();
    public virtual ICollection<ActividadEntity> Actividades { get; set; } = new List<ActividadEntity>();
}

[Table("inscripciones")]
public class InscripcionEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("student_id")]
    public int StudentId { get; set; }

    [Required]
    [Column("course_id")]
    public int CourseId { get; set; }

    [Column("enrollment_date")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "Activo";

    [Column("final_grade")]
    public decimal? FinalGrade { get; set; }

    // Navigation properties
    [ForeignKey("StudentId")]
    public virtual UsuarioEntity Student { get; set; } = null!;

    [ForeignKey("CourseId")]
    public virtual CursoEntity Course { get; set; } = null!;
}

[Table("materiales")]
public class MaterialEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("course_id")]
    public int CourseId { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("type")]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [Column("content_category")]
    [MaxLength(20)]
    public string ContentCategory { get; set; } = "Teorico";

    [Column("file_path")]
    [MaxLength(500)]
    public string? FilePath { get; set; }

    [Column("url")]
    [MaxLength(500)]
    public string? Url { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("CourseId")]
    public virtual CursoEntity Course { get; set; } = null!;
}

[Table("actividades")]
public class ActividadEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("course_id")]
    public int CourseId { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("type")]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [Column("max_score")]
    public decimal MaxScore { get; set; } = 100.00m;

    [Column("due_date")]
    public DateTime? DueDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("CourseId")]
    public virtual CursoEntity Course { get; set; } = null!;
    
    public virtual ICollection<EntregaEntity> Entregas { get; set; } = new List<EntregaEntity>();
}

[Table("entregas")]
public class EntregaEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("activity_id")]
    public int ActivityId { get; set; }

    [Required]
    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("submission_text")]
    public string? SubmissionText { get; set; }

    [Column("file_path")]
    [MaxLength(500)]
    public string? FilePath { get; set; }

    [Column("submitted_at")]
    public DateTime SubmittedAt { get; set; } = DateTime.Now;

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "Entregado";

    // Navigation properties
    [ForeignKey("ActivityId")]
    public virtual ActividadEntity Activity { get; set; } = null!;

    [ForeignKey("StudentId")]
    public virtual UsuarioEntity Student { get; set; } = null!;
    
    public virtual CalificacionEntity? Calificacion { get; set; }
}

[Table("calificaciones")]
public class CalificacionEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("entrega_id")]
    public int EntregaId { get; set; }

    [Required]
    [Column("score")]
    public decimal Score { get; set; }

    [Column("letter_grade")]
    [MaxLength(10)]
    public string? LetterGrade { get; set; }

    [Column("feedback")]
    public string? Feedback { get; set; }

    [Column("graded_by")]
    public int? GradedBy { get; set; }

    [Column("graded_at")]
    public DateTime GradedAt { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("EntregaId")]
    public virtual EntregaEntity Entrega { get; set; } = null!;

    [ForeignKey("GradedBy")]
    public virtual UsuarioEntity? GradedByUser { get; set; }
}

[Table("mensajes")]
public class MensajeEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("sender_id")]
    public int SenderId { get; set; }

    [Required]
    [Column("receiver_id")]
    public int ReceiverId { get; set; }

    [Required]
    [Column("subject")]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("is_read")]
    public bool IsRead { get; set; } = false;

    [Column("sent_at")]
    public DateTime SentAt { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("SenderId")]
    public virtual UsuarioEntity Sender { get; set; } = null!;

    [ForeignKey("ReceiverId")]
    public virtual UsuarioEntity Receiver { get; set; } = null!;
}

[Table("notificaciones")]
public class NotificacionEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("message")]
    public string Message { get; set; } = string.Empty;

    [Column("type")]
    [MaxLength(20)]
    public string Type { get; set; } = "Info";

    [Column("is_read")]
    public bool IsRead { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual UsuarioEntity User { get; set; } = null!;
}

-- =====================================================
-- SISTEMA DE GESTIÓN ACADÉMICA MODULAR
-- Script de creación de base de datos MySQL
-- =====================================================

DROP DATABASE IF EXISTS PlataformaAcademica;
CREATE DATABASE PlataformaAcademica CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE PlataformaAcademica;

-- =====================================================
-- TABLA: usuarios
-- =====================================================
CREATE TABLE usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    role ENUM('Estudiante', 'Profesor', 'Administrador') NOT NULL,
    additional_info VARCHAR(255),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_username (username),
    INDEX idx_role (role)
) ENGINE=InnoDB;

-- =====================================================
-- TABLA: cursos
-- =====================================================
CREATE TABLE cursos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    code VARCHAR(20) UNIQUE NOT NULL,
    name VARCHAR(200) NOT NULL,
    description TEXT,
    credits INT DEFAULT 3,
    instructor_id INT,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (instructor_id) REFERENCES usuarios(id) ON DELETE SET NULL,
    INDEX idx_code (code),
    INDEX idx_instructor (instructor_id)
) ENGINE=InnoDB;

-- =====================================================
-- TABLA: inscripciones
-- =====================================================
CREATE TABLE inscripciones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    student_id INT NOT NULL,
    course_id INT NOT NULL,
    enrollment_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    status ENUM('Activo', 'Completado', 'Retirado') DEFAULT 'Activo',
    final_grade DECIMAL(5,2),
    FOREIGN KEY (student_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (course_id) REFERENCES cursos(id) ON DELETE CASCADE,
    UNIQUE KEY unique_enrollment (student_id, course_id),
    INDEX idx_student (student_id),
    INDEX idx_course (course_id)
) ENGINE=InnoDB;

-- =====================================================
-- TABLA: materiales
-- =====================================================
CREATE TABLE materiales (
    id INT AUTO_INCREMENT PRIMARY KEY,
    course_id INT NOT NULL,
    title VARCHAR(200) NOT NULL,
    description TEXT,
    type ENUM('PDF', 'Video', 'Documento', 'Enlace') NOT NULL,
    content_category ENUM('Teorico', 'Practico') DEFAULT 'Teorico',
    file_path VARCHAR(500),
    url VARCHAR(500),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (course_id) REFERENCES cursos(id) ON DELETE CASCADE,
    INDEX idx_course (course_id),
    INDEX idx_type (type)
) ENGINE=InnoDB;

-- =====================================================
-- TABLA: actividades
-- =====================================================
CREATE TABLE actividades (
    id INT AUTO_INCREMENT PRIMARY KEY,
    course_id INT NOT NULL,
    title VARCHAR(200) NOT NULL,
    description TEXT,
    type ENUM('Tarea', 'Examen', 'Practica', 'Cuestionario') NOT NULL,
    max_score DECIMAL(5,2) DEFAULT 100.00,
    due_date DATETIME,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (course_id) REFERENCES cursos(id) ON DELETE CASCADE,
    INDEX idx_course (course_id),
    INDEX idx_due_date (due_date)
) ENGINE=InnoDB;

-- =====================================================
-- TABLA: entregas
-- =====================================================
CREATE TABLE entregas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    activity_id INT NOT NULL,
    student_id INT NOT NULL,
    submission_text TEXT,
    file_path VARCHAR(500),
    submitted_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    status ENUM('Pendiente', 'Entregado', 'Calificado') DEFAULT 'Entregado',
    FOREIGN KEY (activity_id) REFERENCES actividades(id) ON DELETE CASCADE,
    FOREIGN KEY (student_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    UNIQUE KEY unique_submission (activity_id, student_id),
    INDEX idx_activity (activity_id),
    INDEX idx_student (student_id)
) ENGINE=InnoDB;

-- =====================================================
-- TABLA: calificaciones
-- =====================================================
CREATE TABLE calificaciones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    entrega_id INT NOT NULL,
    score DECIMAL(5,2) NOT NULL,
    letter_grade VARCHAR(10),
    feedback TEXT,
    graded_by INT,
    graded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (entrega_id) REFERENCES entregas(id) ON DELETE CASCADE,
    FOREIGN KEY (graded_by) REFERENCES usuarios(id) ON DELETE SET NULL,
    INDEX idx_entrega (entrega_id)
) ENGINE=InnoDB;

-- =====================================================
-- TABLA: asistencias
-- =====================================================
CREATE TABLE asistencias (
    id INT AUTO_INCREMENT PRIMARY KEY,
    student_id INT NOT NULL,
    course_id INT NOT NULL,
    date DATE NOT NULL,
    status ENUM('Presente', 'Ausente', 'Tardanza', 'Justificado') NOT NULL,
    notes TEXT,
    recorded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (student_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (course_id) REFERENCES cursos(id) ON DELETE CASCADE,
    UNIQUE KEY unique_attendance (student_id, course_id, date),
    INDEX idx_student (student_id),
    INDEX idx_course (course_id),
    INDEX idx_date (date)
) ENGINE=InnoDB;

-- =====================================================
-- TABLA: mensajes
-- =====================================================
CREATE TABLE mensajes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    sender_id INT NOT NULL,
    receiver_id INT NOT NULL,
    subject VARCHAR(200) NOT NULL,
    content TEXT NOT NULL,
    is_read BOOLEAN DEFAULT FALSE,
    sent_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (sender_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (receiver_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    INDEX idx_sender (sender_id),
    INDEX idx_receiver (receiver_id),
    INDEX idx_read (is_read)
) ENGINE=InnoDB;

-- =====================================================
-- TABLA: notificaciones
-- =====================================================
CREATE TABLE notificaciones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    message TEXT NOT NULL,
    type ENUM('Info', 'Warning', 'Success', 'Error') DEFAULT 'Info',
    is_read BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    INDEX idx_user (user_id),
    INDEX idx_read (is_read)
) ENGINE=InnoDB;

-- =====================================================
-- DATOS INICIALES
-- =====================================================

-- Usuarios de demostración (password: todos usan "123456" hasheado)
INSERT INTO usuarios (username, password_hash, email, role, additional_info) VALUES
('admin', '$2a$11$K6XfQqwMxQqKqYqKqYqKqO', 'admin@plataforma.edu', 'Administrador', 'Super Admin'),
('profesor1', '$2a$11$K6XfQqwMxQqKqYqKqYqKqO', 'profesor1@plataforma.edu', 'Profesor', 'Departamento de Sistemas'),
('profesor2', '$2a$11$K6XfQqwMxQqKqYqKqYqKqO', 'profesor2@plataforma.edu', 'Profesor', 'Departamento de Matemáticas'),
('estudiante1', '$2a$11$K6XfQqwMxQqKqYqKqYqKqO', 'estudiante1@plataforma.edu', 'Estudiante', 'EST-001'),
('estudiante2', '$2a$11$K6XfQqwMxQqKqYqKqYqKqO', 'estudiante2@plataforma.edu', 'Estudiante', 'EST-002'),
('estudiante3', '$2a$11$K6XfQqwMxQqKqYqKqYqKqO', 'estudiante3@plataforma.edu', 'Estudiante', 'EST-003');

-- Cursos de demostración
INSERT INTO cursos (code, name, description, credits, instructor_id) VALUES
('CS101', 'Introducción a la Programación', 'Fundamentos de programación en C#', 4, 2),
('CS201', 'Estructuras de Datos', 'Algoritmos y estructuras de datos avanzadas', 4, 2),
('MATH101', 'Cálculo I', 'Límites, derivadas e integrales', 5, 3),
('CS301', 'Bases de Datos', 'Diseño y administración de bases de datos', 3, 2);

-- Inscripciones de demostración
INSERT INTO inscripciones (student_id, course_id, status) VALUES
(4, 1, 'Activo'),
(4, 2, 'Activo'),
(5, 1, 'Activo'),
(5, 3, 'Activo'),
(6, 1, 'Activo'),
(6, 4, 'Activo');

-- Materiales de demostración
INSERT INTO materiales (course_id, title, description, type, content_category, url) VALUES
(1, 'Introducción a C#', 'Video tutorial sobre sintaxis básica', 'Video', 'Teorico', 'https://example.com/video1'),
(1, 'Guía de Variables', 'Documento PDF sobre tipos de datos', 'PDF', 'Teorico', '/materials/cs101_variables.pdf'),
(1, 'Ejercicios Prácticos', 'Problemas de programación', 'Documento', 'Practico', '/materials/cs101_exercises.pdf'),
(2, 'Listas y Arreglos', 'Teoría sobre estructuras lineales', 'PDF', 'Teorico', '/materials/cs201_lists.pdf');

-- Actividades de demostración
INSERT INTO actividades (course_id, title, description, type, max_score, due_date) VALUES
(1, 'Tarea 1: Hola Mundo', 'Crear tu primer programa en C#', 'Tarea', 100.00, '2025-12-15 23:59:59'),
(1, 'Examen Parcial', 'Evaluación de conceptos básicos', 'Examen', 100.00, '2025-12-20 14:00:00'),
(2, 'Implementar Lista Enlazada', 'Crear una lista enlazada desde cero', 'Practica', 100.00, '2025-12-18 23:59:59');

-- =====================================================
-- VISTAS ÚTILES
-- =====================================================

-- Vista: Estudiantes con sus cursos
CREATE VIEW vista_estudiantes_cursos AS
SELECT 
    u.id as student_id,
    u.username,
    u.email,
    c.code as course_code,
    c.name as course_name,
    i.status,
    i.final_grade
FROM usuarios u
JOIN inscripciones i ON u.id = i.student_id
JOIN cursos c ON i.course_id = c.id
WHERE u.role = 'Estudiante';

-- Vista: Resumen de cursos
CREATE VIEW vista_resumen_cursos AS
SELECT 
    c.id,
    c.code,
    c.name,
    c.credits,
    u.username as instructor_name,
    COUNT(DISTINCT i.student_id) as total_students,
    COUNT(DISTINCT m.id) as total_materials,
    COUNT(DISTINCT a.id) as total_activities
FROM cursos c
LEFT JOIN usuarios u ON c.instructor_id = u.id
LEFT JOIN inscripciones i ON c.id = i.course_id
LEFT JOIN materiales m ON c.id = m.course_id
LEFT JOIN actividades a ON c.id = a.course_id
GROUP BY c.id;

-- =====================================================
-- FIN DEL SCRIPT
-- =====================================================

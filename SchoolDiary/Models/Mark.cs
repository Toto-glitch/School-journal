using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDiary.Models
{
    /// <summary>
    /// Таблица оценок
    /// </summary>
    [Table("Marks")]
    public class Mark
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int Value { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [MaxLength(200)]
        public string Comment { get; set; }

        // Связь с учеником
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        // Связь с предметом
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        // Связь с учителем (кто выставил оценку)
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        // Лог изменений
        public virtual ICollection<MarkLog> Logs { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}

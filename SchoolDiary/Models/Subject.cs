using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDiary.Models
{
    /// <summary>
    /// Таблица предметов
    /// </summary>
    [Table("Subjects")]
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Связь с учителем (один предмет - один учитель по заданию)
        public int? TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        // Оценки по предмету
        public virtual ICollection<Mark> Marks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

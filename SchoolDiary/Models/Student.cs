using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDiary.Models
{
    /// <summary>
    /// Таблица учеников
    /// </summary>
    [Table("Students")]
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [MaxLength(20)]
        public string Class { get; set; } // Например, "9А"

        // Связь с пользователем (для авторизации ученика)
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        // Связь с родителем (один родитель - один ученик по заданию)
        public int? ParentId { get; set; }
        public virtual Parent Parent { get; set; }

        // Оценки ученика
        public virtual ICollection<Mark> Marks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

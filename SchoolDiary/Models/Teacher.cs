using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDiary.Models
{
    /// <summary>
    /// Таблица учителей
    /// </summary>
    [Table("Teachers")]
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string Position { get; set; }

        // Связь с пользователем (для авторизации учителя)
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        // Предмет, который ведёт учитель
        public virtual Subject Subject { get; set; }

        // Оценки, выставленные учителем
        public virtual ICollection<Mark> Marks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

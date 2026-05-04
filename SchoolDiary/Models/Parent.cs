using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDiary.Models
{
    /// <summary>
    /// Таблица родителей
    /// </summary>
    [Table("Parents")]
    public class Parent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        // Связь с пользователем (для авторизации родителя)
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        // Ребёнок (один родитель - один ученик по заданию)
        public virtual Student Student { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

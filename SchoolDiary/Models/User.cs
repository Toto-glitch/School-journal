using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDiary.Models
{
    /// <summary>
    /// Таблица пользователей системы (для авторизации)
    /// </summary>
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } // Director, Teacher, Parent, Student

        // Навигационные свойства (связи 1:1)
        public virtual Teacher Teacher { get; set; }
        public virtual Parent Parent { get; set; }
        public virtual Student Student { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

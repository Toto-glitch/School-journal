using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolDiary.Models
{
    /// <summary>
    /// Таблица логов изменений оценок (для триггера/отслеживания)
    /// </summary>
    [Table("MarkLogs")]
    public class MarkLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MarkId { get; set; }
        public virtual Mark Mark { get; set; }

        [Required]
        public int OldValue { get; set; }

        [Required]
        public int NewValue { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }

        [Required]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string Action { get; set; } // Insert, Update, Delete
    }
}

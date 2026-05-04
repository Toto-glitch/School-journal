using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolJournal.Model
{
    [Table("mark_logs")]
    public class MarkLog
    {
        [Key, Column("mark_log_id")]
        public int Id { get; set; }

        [Column("old_value")]
        public int OldValue { get; set; }

        [Required, Column("new_value")]
        public int NewValue { get; set; }

        [Column("changed_at")]
        public DateTime ChangedAt { get; set; } = DateTime.Now;

        [Column("reason_text")]
        public string Reason { get; set; }

        public virtual Mark Mark { get; set; }
        public virtual User User { get; set; }
    }
}
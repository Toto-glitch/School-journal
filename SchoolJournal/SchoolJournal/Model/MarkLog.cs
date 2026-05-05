using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolJournal.Model
{
    public class MarkLog
    {
        [Key]
        public int Id { get; set; }

        public int MarkId { get; set; }
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        public int OldValue { get; set; }
        public int NewValue { get; set; }
        public DateTime ChangeDate { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [MaxLength(100)]
        public string Action { get; set; }
    }
}
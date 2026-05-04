using System;

namespace SchoolJournal.Model
{
    public class MarkLog
    {
        public int Id { get; set; }
        public int OldValue { get; set; }
        public int NewValue { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;
        public string Reason { get; set; }

        public int MarkId { get; set; }
        public virtual Mark Mark { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
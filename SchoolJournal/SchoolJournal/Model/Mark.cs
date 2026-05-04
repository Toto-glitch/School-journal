using System;
using System.Collections.Generic;

namespace SchoolJournal.Model
{
    public class Mark
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Comment { get; set; }

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<MarkLog> Logs { get; set; } = new List<MarkLog>();
    }
}

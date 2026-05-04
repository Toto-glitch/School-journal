using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;

namespace SchoolJournal.Model
{
    [Table("marks")]
    public class Mark
    {
        [Key, Column("mark_id")]
        public int Id { get; set; }

        [Range(1, 5, ErrorMessage = "Mark can be in range(1, 5)"), Column("value")]
        public int Value { get; set; }

        [Column("created_at")]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Column("comment_text")]
        public string Comment { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<MarkLog> Logs { get; set; } = new List<MarkLog>();
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolJournal.Model
{
    [Table("subjects")]
    public class Subject
    {
        [Key, Column("subject_id")]
        public int Id { get; set; }

        [Required, MaxLength(255), Column("subject_name")]
        public string Name { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolJournal.Model
{
    [Table("groups")]
    public class Group
    {
        [Key, Column("group_id")]
        public int Id { get; set; }

        [Required, MaxLength(10), Column("group_name")]
        public string Name { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}